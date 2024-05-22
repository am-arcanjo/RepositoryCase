@echo off
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.App\bin\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.App\obj\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Business\bin\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Business\obj\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Dominio\bin\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Dominio\obj\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Infra\bin\Debug\" git rm --cached "%i"
)
FOR /R %i IN (*.dll) DO (
    IF "%~dp$i"=="%cd%\RepositorioGitHub.Infra\obj\Debug\" git rm --cached "%i"
)