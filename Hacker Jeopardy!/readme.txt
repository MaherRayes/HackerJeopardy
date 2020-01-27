How to change some aspects of the game:

Changing the game’s aesthetics is very easy through unity’s interface. It is possible to change images, colors, texts and adding more elements to the game’s look without effecting anything in the code.

However, it is important when adding anything that is connected to the game’s logic (for example the buttons) to copy every component from the old object to the new one and putting the object in the right object group (parent).
To avoid these issues, try not to add new objects, rather change existing objects or duplicate them to get the desired changes.

It is also relatively easy to change some fixed variable like the amount of money for each button (now fixed for 100 – 500$ or 200-1000$) in the clue class. Also it is possible to change the currency used (in the MoneyButton Class).

Regarding the game creation and game logic. It is quite hard to change these aspects considering that they are connected to a lot of other aspects in the game.

The scripts responsible mainly for the game logic are:
Manager
GameSetup
FinalJeopardy

The scripts responsible mainly for the game creation/editing are:
GameEditor
SQLiteManager

Plugins used:
AudioImporter
GifPlayer
SimpleFileBrowser
ProAudioPlayer