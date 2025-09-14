using Godot;
using System;
using System.IO;

public partial class ProjectSelectionScreen : Node
{
    [ExportCategory("Project Screen")]
    [Export] private PackedScene _projectScreen;
    
    [ExportCategory("Project Buttons")]
    [Export] private Button _createProjectButton;
    [Export] private Button _openProjectButton;
    [Export] private Button _setProjectPathButton;
    [Export] private Button _createProjectFinalButton;
    [Export] private Button _cancelCreateProjectButton;
    [Export] private Button _exitAppButton;
    
    [ExportCategory("Project Labels and text inputs")]
    [Export] private Label _setProjectPathButtonLabel;
    [Export] private TextEdit _setProjectNameTextInput;
    [Export] private RichTextLabel _mainCtaTextBox;

    [ExportCategory("Screen elements")]
    [Export] private Control _baseMainScreen;
    [Export] private VBoxContainer _initialSelectionContainer;
    [Export] private VBoxContainer _createNewProjectContainer;
    [Export] private Control _loadingProjectSubScreen;
    [Export]  private FileDialog _newProjectFolderDialog;
    [Export] private FileDialog _openProjectFolderDialog;
    
    [ExportCategory("Default CTAs")]
    [Export] private string _setProjectPathDefaultCta;
    [Export] private string _openingDefaultCta;
    [Export] private string _createProjectDefaultCta;
    
    [ExportCategory("Predefined Colors")]
    [Export] Color _invisibleColor = new Color(255, 255, 255, 0);
    [Export] Color _visibleColor = new Color(255, 255, 255, 255);

    private string _selectedProjectFolderPath;
    private string _currentFullProjectPath;
    private string _newProjectName;
    private AppState _appState;

    public override void _Ready()
    {
        GD.Print("Project Selection Screen");
        
        SetAppState();
        SetBindings();
        SetInitialStates();
        
        
    }

    private void SetAppState()
    {
        _appState = (AppState)GetNode("/root/AppState");

        if (_appState == null)
        {
            GD.PrintErr("AppState not found");
        }
        
    }

    private void SetBindings()
    {
        _newProjectFolderDialog.DirSelected += OnNewProjectFolderSelected; 
        _openProjectFolderDialog.FileSelected += OnOpenProjectPathSelected;
        _setProjectPathButton.Pressed += OpenFolderDialogForNewProject;
        _setProjectNameTextInput.TextChanged += OnNewProjectNameTextInputChanged;
        _createProjectButton.Pressed += OnCreateNewProjectButtonPressed;
        _createProjectFinalButton.Pressed += OnCreateNewProjectFinalButtonPressed;
        _cancelCreateProjectButton.Pressed += OnCancelCreateProjectButtonPressed;
        _exitAppButton.Pressed += OnExitAppButtonPressed;
        _openProjectButton.Pressed += OnOpenProjectButtonPressed;
        
    }

    private void SetInitialStates()
    {
        _setProjectPathButtonLabel.Text = _setProjectPathDefaultCta;
        _createProjectFinalButton.SetModulate(_invisibleColor);
        _cancelCreateProjectButton.SetVisible(false);
        _initialSelectionContainer.SetVisible(true);
        _createNewProjectContainer.SetVisible(false);
        _mainCtaTextBox.Text = _openingDefaultCta;
    }

    private void OnOpenProjectButtonPressed()
    {
        _openProjectFolderDialog.PopupCentered(new Vector2I(800, 600));
    }

    private void OnOpenProjectPathSelected(string projectPath)
    {
        GD.Print(projectPath);
        _baseMainScreen.SetVisible(false);
        _loadingProjectSubScreen.SetVisible(true);
        _currentFullProjectPath = projectPath;
        OpenProjectScreen();
    }

    private void OnExitAppButtonPressed()
    {
        GetTree().Quit();
    }

    private void SetWindowsSettings()
    {
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
    }

    private void OnNewProjectFolderSelected(string folderPath)
    {
        GD.Print("Selected folder: " + folderPath);
        _setProjectPathButtonLabel.Text = folderPath;
        _selectedProjectFolderPath = folderPath;
        CheckForProjectCreationSettingsCompleted();
    }

    private void OpenFolderDialogForNewProject()
    {
        _newProjectFolderDialog.PopupCentered(new Vector2I(800, 600));
    }

    private void OnNewProjectNameTextInputChanged()
    {
        GD.Print("Project Name is : " + _setProjectNameTextInput.Text);
        _newProjectName = _setProjectNameTextInput.Text;
        CheckForProjectCreationSettingsCompleted();
    }

    private void CheckForProjectCreationSettingsCompleted()
    {
        if (string.IsNullOrEmpty(_selectedProjectFolderPath) || string.IsNullOrEmpty(_newProjectName))
        {
            _createProjectFinalButton.SetModulate(_invisibleColor);
            return; 
        }
        
        _createProjectFinalButton.SetModulate(_visibleColor);
        
    }

    private void OnCreateNewProjectButtonPressed()
    {
        _initialSelectionContainer.SetVisible(false);
        _createNewProjectContainer.SetVisible(true);
        _cancelCreateProjectButton.SetVisible(true);
        _mainCtaTextBox.Text = _createProjectDefaultCta;

    }
    private void OnCreateNewProjectFinalButtonPressed()
    {
        _baseMainScreen.SetVisible(false);
        _loadingProjectSubScreen.SetVisible(true);
        string fileName = _newProjectName + ".fksp";
        _currentFullProjectPath = Path.Combine(_selectedProjectFolderPath, fileName);
        GD.Print("Creating new project " + _newProjectName + " in folder: " + _selectedProjectFolderPath);
        GD.Print("Full path: " + _currentFullProjectPath);

        try
        {
            
            if (!Directory.Exists(_selectedProjectFolderPath))
            {
                GD.PrintErr("Folder does not exist: " + _selectedProjectFolderPath);
                return;
            }

            
            using (FileStream fs = File.Create(_currentFullProjectPath))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("FKSP Project File");
                    writer.WriteLine("ProjectName=" + _newProjectName);
                    writer.WriteLine("Created=" + System.DateTime.Now);
                }
            }

            GD.Print("Project file created successfully at: " + _currentFullProjectPath);
            OpenProjectScreen();
            
        }
        catch (Exception ex)
        {
            GD.PrintErr("Error creating project file: " + ex.Message);
        }
    }

    private void OnCancelCreateProjectButtonPressed()
    {
        _initialSelectionContainer.SetVisible(true);
        _createNewProjectContainer.SetVisible(false);
        _cancelCreateProjectButton.SetVisible(false);
        _mainCtaTextBox.Text = _openingDefaultCta;
    }
    
    private  async void OpenProjectScreen()
    {
        await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
        if (_projectScreen == null || _appState == null) return;
        _appState.SetCurrentProjectPath(_currentFullProjectPath);
        _appState.ChangeScene(_projectScreen);
    }
    
    
    
    
    


}
