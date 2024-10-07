<p align="center">
  <img width="256" heigth="256" src="docs\vmup.png">
<h1 align="center">VMUnprotect.Dumper</h1>
<p align="center">
  <strong>VMUnprotect.Dumper</strong> is a project engaged in hunting tampered <strong>VMProtect assemblies</strong>. It makes use of <a href="https://github.com/pardeike/Harmony">AsmResolver</a> to dynamically unpack <strong>VMP</strong> protected assembly. Works on <a href="https://vmpsoft.com/20220827/vmprotect-3-7/">VMProtect 3.7.0</a> (Latest) and few versions back.
</p>
</p>
<p align="center">
  <img src="https://forthebadge.com/images/badges/built-with-love.svg" alt="appveyor-ci" />
  <img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg" alt="appveyor-ci" />
</p>
</p>

</br></br>
# Before usage of VMUnprotect.Dumper
<img src="docs/protected.png" >

</br></br>

# After usage of VMUnprotect.Dumper
<img src="docs/decrypted.png" >

# Usage
```sh
VMUnprotect.Dumper.exe example.vmp.exe
```
> If the target app has a different framework, it probably won't work. Compile it yourself for your needs. <br>

### Checkout also:
* [VMUnprotect](https://github.com/void-stack/VMUnprotect) - VMUnprotect can dynamically log and manipulate calls from virtualized methods by VMProtect.
* [DeMutation](https://github.com/xlfj5211/DeMutation) - VMProtect Mutation Cleaner/Remover
* [VMP-Demutator](https://github.com/xlfj5211/VMP-Demutator) - Demutator for VMP Mutations
* [VMPBypass](https://github.com/CabboShiba/VMPBypass) - This tool will allow you to bypass VMProtect .NET AntiDebugger. Made by Cabbo.
* [VMPStrings-Decrypter](https://github.com/CabboShiba/VMPStrings-Decrypter) - Decrypt VMProtect (.NET) obfuscated strings. Made by Cabbo with love.
* [VMPImportFixer-1](https://github.com/coders-vechai/VMPImportFixer-1?) - VMProtect .NET Import Protection Fixer
* [VMPResources](https://github.com/coders-vechai/VMPResources)
* [VMPStrings](https://github.com/coders-vechai/VMPStrings)
* [VMP.NET-Kill](https://github.com/DarkBullNull/VMP.NET-Kill) - This program help you with unpacking vmprotect

# Credits
* [wwh1004](https://github.com/wwh1004) - Idea.
* [(Discord) MrToms#1244]() - Resources

This tool uses the following (open source) software:
* [AsmResolver](https://github.com/Washi1337/AsmResolver) by [Washi](https://github.com/Washi1337), licensed under the MIT license, for reading/writing assemblies.

## 💵 Want to buy me a Coffee?
     - Donate BTC at `bc1q048wrqztka5x2syt9mtj68uuf73vqry60s38vf`
     - Donate ETH at `0x86b2C17C94A1E6f35d498d17a37dc1f8A715139b`
