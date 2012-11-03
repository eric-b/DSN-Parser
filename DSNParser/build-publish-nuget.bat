@echo off
nuget pack $(ProjectPath) -Prop Configuration=Release
rem nuget push

pause
