using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AsmResolver.DotNet;
using AsmResolver.IO;
using AsmResolver.PE.DotNet.ReadyToRun;
using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using HarmonyLib;
using Sharprompt;

namespace VMProtect.Dumper
{
	internal class Program
	{
		internal const string asciiArt = @"
_________                __                 
\_   ___ \  ____________/  |_  ____ ___  ___
/    \  \/ /  _ \_  __ \   __\/ __ \\  \/  /
\     \___(  <_> )  | \/|  | \  ___/ >    < 
 \______  /\____/|__|   |__|  \___  >__/\_ \
        \/                        \/      \/
                VMUnprotect.Dumper
          https://github.com/MasterDomino/VMUnprotect.Dumper
            Credits: wwh1004, MrToms, void-stack
";

		[HarmonyPatch(typeof(Marshal), nameof(Marshal.AllocHGlobal), [typeof(int)])]
		internal static class MarshalHook
		{
			[STAThread]
			internal static bool Prefix(int cb)
			{
				try
				{
					cb--;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error during AntiDBG Bypass:\n{ex.Message}");
					cb = 1;
				}
				return false;
			}
		}

		internal static void Main(string[] args)
		{
			Console.Title = "VMUnprotect.Dumper";
			Console.WriteLine(asciiArt);

			string? target;

			if (args.Length > 0 && File.Exists(args[0]))
			{
				target = Prompt.Input<string>("Enter file path", args[0]);
			}
			else
			{
				target = Prompt.Input<string>("Enter file path");
			}

			// adjust path to absolute, required by Assembly.LoadFile();
			target = Path.GetFullPath(target);

			if (File.Exists(target))
			{
				string fileName = $"{Path.GetFileNameWithoutExtension(target)}-decrypted.exe";

				// Try to load assembly and gather the ManifestModule
				Assembly? assembly;

				try
				{
					assembly = Assembly.LoadFile(target);

					// TODO: if assembly has anti-debug implemented this wont throw an error,
					// but instead just close the app instance, we want to somehow catch that before
					// this only happens if anti-debug checks on the beginning of .cctor
					// this can be circumvented with Lib.Harmony patch

					// !!! CREDITS: CabboShiba !!!
					Harmony patch = new("VMPRotectAntiDebuggerBypass_https://github.com/CabboShiba");
					patch.PatchAll(Assembly.GetExecutingAssembly());

				}
				catch (BadImageFormatException)
				{
					Console.WriteLine("Target app probably has a different framework.");
					return;
				}
				catch(Exception ex)
				{
					Console.WriteLine($"Could not load {target}\n{ex}");
					return;

				}

				Module manifestModule = assembly.ManifestModule;

				// Quick load for .cctor search
				ModuleDefinition module = ModuleDefinition.FromFile(target);

				// Resolve MethodBase of .cctor where vmp initializes itself
				MethodBase? cctor =
					assembly.ManifestModule.ResolveMethod(module.TopLevelTypes[0].GetStaticConstructor()!.MetadataToken.ToInt32());

				// Get Module Base Address from loaded assembly
				nint hInstance = Marshal.GetHINSTANCE(manifestModule);

				// Make sure static constructor exists
				if (cctor is not null)
				{
					// Force VMProtect to fix methods
					RuntimeHelpers.PrepareMethod(cctor.MethodHandle);

					// Load PEFile from disk
					PEFile diskImage = PEFile.FromFile(target);

					// Load decrypted PEFile from module base
					PEFile runtimeImage = PEFile.FromModuleBaseAddress(hInstance);

					OptionalHeader optionalHeader = runtimeImage.OptionalHeader;
					optionalHeader.Magic = diskImage.OptionalHeader.Magic; // Fix the incorrect magic
					optionalHeader.AddressOfEntryPoint = diskImage.OptionalHeader.AddressOfEntryPoint; // Fix the incorrect AddressOfEntrypoint

					// try to do onje of the following to remove unnecessary garbage
					//runtimeImage.UpdateHeaders();
					//runtimeImage.AlignSections();

					// Write fixed runtimeImage to disk
					using (FileStream fs = File.Create(fileName))
					{
						runtimeImage.Write(new BinaryStreamWriter(fs));
					}

					Console.WriteLine($"Saved as: {Path.GetFullPath(fileName)}");
				}
				else
				{
					Console.WriteLine("Failed to prepare .cctor");
				}
			}
			else
			{
				Console.WriteLine("File either doesn't exist or you didn't provide it (VMProtect.Dumper File.exe)");
			}

			Console.ReadKey();
		}
	}
}
