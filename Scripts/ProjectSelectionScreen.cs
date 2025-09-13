using Godot;
using System;

public partial class ProjectSelectionScreen : Node
{
    [ExportCategory("Project Buttons")]
    [Export] private Button _createProjectButton;
    [Export] private Button _openProjectButton;
    [Export] private Button _setProjectPathButton;
    [Export] private Button _createProjectFinalButton;
    [Export] private Button _cancelCreateProjectButton;
    
    [ExportCategory("Project Labels and text inputs")]
    [Export] private Label _setProjectPathButtonLabel;
    [Export] private TextEdit _setProjectNameTextInput;
    [Export] private RichTextLabel _mainCtaTextBox;
    
    [ExportCategory("Screen elements")]
    [Export] private VBoxContainer _initialSelectionContainer;
    [Export] private VBoxContainer _createNewProjectContainer;
    [Export]  private FileDialog _folderDialog;
    
    [ExportCategory("Default CTAs")]
    [Export] private string _setProjectPathDefaultCta;
    [Export] private string _openingDefaultCta;
    [Export] private string _createProjectDefaultCta;
    
    [ExportCategory("Predefined Colors")]
    [Export] Color _invisibleColor = new Color(255, 255, 255, 0);
    [Export] Color _visibleColor = new Color(255, 255, 255, 255);

    private string _selectedProjectPath;
    private string _newProjectName;

    public override void _Ready()
    {
        if (_folderDialog != null)
        {
            _folderDialog.DirSelected += OnFolderSelected; 
        }

        if (_setProjectPathButton != null)
        {
            _setProjectPathButton.Pressed += OpenFolderDialog;
            
            _setProjectPathButtonLabel.Text = _setProjectPathDefaultCta;
        }

        if (_setProjectNameTextInput != null)
        {
            _setProjectNameTextInput.TextChanged += OnNewProjectNameTextInputChanged;
        }
        
        if (_createProjectButton != null)
        {
            _createProjectButton.Pressed += OnCreateNewProjectButtonPressed;
        }

        if (_createProjectFinalButton != null)
        {
            _createProjectFinalButton.SetModulate(_invisibleColor);

            _createProjectFinalButton.Pressed += OnCreateNewProjectFinalButtonPressed;
        }

        if (_cancelCreateProjectButton != null)
        {
            _cancelCreateProjectButton.Pressed += OnCancelCreateProjectButtonPressed;
            _cancelCreateProjectButton.SetVisible(false);
        }

        if (_initialSelectionContainer != null)
        {
            _initialSelectionContainer.SetVisible(true);
        }

        if (_createNewProjectContainer != null)
        {
            _createNewProjectContainer.SetVisible(false);
        }

        _mainCtaTextBox.Text = _openingDefaultCta;






    }

    private void OnFolderSelected(string folderPath)
    {
        GD.Print("Selected folder: " + folderPath);
        
        _setProjectPathButtonLabel.Text = folderPath;
        
        _selectedProjectPath = folderPath;
        
        CheckForProjectCreationSettingsCompleted();
        
    }

    private void OpenFolderDialog()
    {
        _folderDialog.PopupCentered(new Vector2I(800, 600));
    }

    private void OnNewProjectNameTextInputChanged()
    {
        GD.Print("Project Name is : " + _setProjectNameTextInput.Text);
        
        _newProjectName = _setProjectNameTextInput.Text;
        
        CheckForProjectCreationSettingsCompleted();
    }

    private void CheckForProjectCreationSettingsCompleted()
    {
        if (string.IsNullOrEmpty(_selectedProjectPath) || string.IsNullOrEmpty(_newProjectName))
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
        GD.Print("Creating new project " + _newProjectName + "in folder : " + _selectedProjectPath);
    }

    private void OnCancelCreateProjectButtonPressed()
    {
        _initialSelectionContainer.SetVisible(true);
        _createNewProjectContainer.SetVisible(false);
        _cancelCreateProjectButton.SetVisible(false);
        _mainCtaTextBox.Text = _openingDefaultCta;
    }
    
    
    
    
    


}
