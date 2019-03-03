

# <img id="logo" src="logo.svg" alt="drawing" width="150" style="float:right;margin-top:-40px;" /> Jarvis Brain

asp.net core c# application to spin all the data around

## Database first - generate db context
```
dotnet ef dbcontext scaffold "Data Source=C:\\source\\jarvis_brain\\jarvis_brain.sqlite" Microsoft.EntityFrameworkCore.Sqlite --context Jarvis_BrainDBContext --context-dir Models --force
```
**There's something wrong in this.**
**It's not applying into the namespace correctly.**
**It should be in ```Jarvis_Brain.Models``` but ends up in ``Jarvis_Brain`` and while I agree the DB context is important, it's not _that_ bloody important!**

### Notes

 - It would be really nice if I could stop the db scaffolder from bringing in the connection string. Thats a bit clunky
  - really needs some sort of default page, some reason to use the logo
    - oooh I wonder if I shoudl create some pointless ascii version?! that would be WAY cool..



<style>#logo {-webkit-animation: rotation 60s infinite linear;}@-webkit-keyframes rotation {from {-webkit-transform: rotate(0deg)}to {-webkit-transform: rotate(-359deg);}}</style>