[Setup]
AppName=Compose
AppVersion=0.1.0
AppVerName=Compose 0.1.0
AppPublisher=Adrian L Lange
AppPublisherURL=https://github.com/p3lim/Compose#readme
DefaultDirName={pf}\Compose
DefaultGroupName=Compose
AllowNoIcons=yes
LicenseFile=D:\Code\repos\Compose\LICENSE
OutputDir=D:\Code\repos\Compose\bin\Release
OutputBaseFilename=Compose-0.1.0
Compression=lzma
SolidCompression=yes

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: " "; Flags: unchecked
Name: "runonstartup"; Description: "{cm:RunOnStartup,Compose}"; GroupDescription: " "; Flags: checkablealone

[CustomMessages]
RunOnStartup=Run %1 on startup

[Files]
Source: "D:\Code\repos\Compose\bin\Debug\Compose.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Code\repos\Compose\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Code\repos\Compose\README.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Compose"; Filename: "{app}\Compose.exe"
Name: "{group}\{cm:ProgramOnTheWeb,Compose}"; Filename: "https://github.com/p3lim/Compose#readme"
Name: "{group}\{cm:UninstallProgram,Compose}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Compose"; Filename: "{app}\Compose.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Compose.exe"; Description: "{cm:LaunchProgram,Compose}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: dirifempty; Name: "{app}"

[Code]
const
	RegPath = 'Software\Microsoft\Windows\CurrentVersion\Run';

procedure CurStepChanged(CurStep: TSetupStep);
begin
	if CurStep = ssPostInstall then begin
		if IsTaskSelected('runonstartup') then
			RegWriteStringValue(HKCU, RegPath, 'Compose', '"' + ExpandConstant('{app}') + '\Compose.exe"');
		if not IsTaskSelected('runonstartup') and RegValueExists(HKCU, RegPath, 'Compose') then
			RegDeleteValue(HKCU, RegPath, 'Compose');
	end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
	if CurUninstallStep = usUninstall then begin
		if RegValueExists(HKCU, RegPath, 'Compose') then
			RegDeleteValue(HKCU, RegPath, 'Compose');
	end;
end;
