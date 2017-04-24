# UWPMaterialControls-

## MaterialTextBox
Tries to implement the material design for text fields documented at [material.io](https://material.io)
### Features

- Text (simple text input)
- Label (depending on focus and text input is above or in top of the Text)
- Placeholder text (shows when control is in focus and the text input is empty)
- Auto complete text (shows when control is in focus, enter button to auto complete, custom event fires, auto complete > placeholder)
- Some Animations (visual feedback when in focus or hovering)
- Invalid state - control changes colour and an error text is shown (if supplied)
- Max. character count
- Segoe MDL2 Assets that are displayed next to the control
- Helper text below the control
- Button to clear text input

Watch it in action - [imgur .gif](http://imgur.com/a/W20gm)


### Properties that can be defined in XAML and code behind

- AutocompleteText
- Text
- LabelText
- LabelFontSize
- HelperText
- ErrorText
- Valid
- PrimaryTextForeground (used for Text)
- SecondaryTextForeground (used for Label, HelperText, CharacterCountPresenter)
- InputLineForeground
- PlaceholderText
- MaxCharacter
- SegoeMDL2Icon

### Events

- TextChanged
- TextAutoCompleted
- ToManyCharacters
