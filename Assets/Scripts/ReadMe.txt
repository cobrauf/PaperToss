How to add another Unlockable item----------------

-add new button in unlockable canvas, add new tag to it
-assign all references in unlockableMenu script in editor
-assign new btn in the ScrollSnap script
-update EnableItem function in UnlockableMenu script

-update all functions related to unlockables in PlayerPrefs
-update ItemUnlockAnimation function in InGameMenuManager script 
-don't forget to update the text portion
-update NextThrowObj func in ThrowObjSwitcher script

If adding new materials:
-create new materials
-add to array in GameManager
-update # in MaterialSwitcher script

If adding new object to throw.............
