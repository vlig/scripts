--http://www.maxforums.org/threads/hex_triplet_color_picker_3dsmax_material_editor/0001.aspx
--"
--Copy/paste that code to a new script window, evaluate once (ctrl+e) and close without saving.
--Then from the Customize User Interface dialog, Keyboard panel, browse to the Mesh Infos category
--and assign a keybord shortcut to Hex Color from Clipboard (I used ctrl+alt+x).
--I'm just using ctrl+c in Phototshop to grab the hex value and ctrl+alt+x in max to launch the macro.
--"

macroScript hexColorFromClipboard
category:"Mesh Infos"
toolTip:"Hex Color from Clipboard"

(
  global rltHexColorFromClipboard
  try destroyDialog rltHexColorFromClipboard catch()

  rollout rltHexColorFromClipboard "Hex Color" width:110
  (
    colorPicker colPick color:gray align:#center fieldWidth:80

    on rltHexColorFromClipboard open do
    (
      cbClass = dotNetClass "System.Windows.Forms.Clipboard"
      local cb = cbClass.GetText()

      try
      (
        local colRed = ("0x" + substring cb 1 2) as integer
        local colGreen = ("0x" + substring cb 3 2) as integer
        local colBlue = ("0x" + substring cb 5 2) as integer
        colPick.color = color colRed colGreen colBlue
      )
      catch
      (
        messageBox "No Hex Color in Clipboard!"
        destroyDialog rltHexColorFromClipboard
      )
    )
  )

  createDialog rltHexColorFromClipboard
)
