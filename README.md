# GlobeTrottersFix
A simple BepinEx mod for Bits & Bops that "fixes" the flip cue in Globe Trotters to not flip back prematurely. It also adds a bunch of extra events in the mixtape editor for Globe Trotters, mainly to liven up the video I wanted to make demonstrating the mod. 

Demo: https://youtu.be/Gwe0nWNgCxU

## Installation
- Install [BepInEx 5.x](https://docs.bepinex.dev/articles/user_guide/installation/index.html) in Bits & Bops.
- Download `GlobeTrottersFix.dll` from the latest [release](https://github.com/AnonUserGuy/GlobeTrottersFix/releases/), and place it in `BepinEx\plugins\`.

## Usage
### Custom Mixtapes
All `swap` events in Globe Trotters and Globe Trotters (Fire) will no longer prematurely end. Now, the marchers will only swap planets once another `step` cue has passed.

### Custom Mixtape Events
The following mixtape events are added to Globe Trotters and Globe Trotters (Fire):
| Name                           | Properties    | Property Types| Description   |
| ------------------------------ | ------------- | ------------- | ------------- |
| `_zoomOutRotateBackgroundSlow` | `_beatOffset` | float         | <p>Camera zooms out and background rotates.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_rotateBackgroundSlow`        | `_beatOffset` | float         | <p>Camera zooms in and background rotates.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_zoomOutRotatePlanets`        | `_beatOffset` | float         | <p>Camera zooms out and planets rotate.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_rotatePlanets`\*             | `_beatOffset` | float         | <p>Camera zooms in and planets rotate.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_zoomOutMultiplePlanets`      | `_beatOffset` | float         | <p>Camera zooms out and shows multiple planets rotating.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_rotateBackgroundFast`\*      | `_beatOffset` | float         | <p>Camera zooms in and background rotates, twice as fast.</p> <p>`_beatOffset` controls offset of rotation animation.</p> |
| `_zoomIntro1`                  | ---           | ---           | <p>Camera zooms out to first scene of Globe Trotters intro.</p> |
| `_zoomIntro2`                  | ---           | ---           | <p>Camera zooms out to second scene of Globe Trotters intro.</p> |
| `_zoomIntro3`                  | ---           | ---           | <p>Camera zooms out to third scene of Globe Trotters intro.</p> |
| `_zoomClose`                   | ---           | ---           | <p>Camera zooms in to default view.</p> |
| `_zoomEnd1`\*                  | ---           | ---           | <p>Camera zooms out to first scene of Globe Trotters outro. Breaks a lot of other zoom out events once called.</p> |
| `_zoomEnd2`\*                  | ---           | ---           | <p>Camera zooms out to second scene of Globe Trotters outro.</p> |
| `_zoomManual`                  | `x`, `y`, `z` | Vector3       | <p>Move camera to arbitrary positon.</p> <p>`x`, `y`, `z` specify the coordinates to move to.</p> |
| `_setState`                    | <p>`_beatOffset`</p><p>`state`</p><p>`zoom`</p><p>`x`, `y`, `z`</p> | <p>float</p><p>string</p><p>string</p><p>Vector3</p> | <p>Sets the state of the camera. Controls both the background/planet movement and camera zoom.</p> <p>`_beatOffset` controls offset of rotation animation. </p> <p>`_beatOffset` controls offset of rotation animation.</p> <p>`state` controls the background/planet animation type.</p> <p>`zoom` controls the zoom level.</p> <p>`x`, `y`, `z` specify the coordinates to move to if camera zoom is set to "manual".</p> |
| `_planetDistance`              | `d`           | float         | <p>Control the distance between the pair of planets at the normal zoom level.</p> <p>`d` specifies the distance. <ul><li>`9.150001f` most of the time</li><li>`11.35f` for `_rotatePlanets`.</li></ul></p> |
| `_flip`                        | <p>`which`</p><p>`flip`</p><p>`immediate`</p> | <p>int</p><p>Boolean</p><p>Boolean</p> | <p>Forces the marchers to flip planets.</p> <p>`which` specifies which marcher <ul><li>`-1` for all marchers.</li> <li>`-2` for all marchers but the player.</li> <li>`0` for just the player.</li> <li>`1` for just the left NPC marcher.</li> <li>`2` for just the right NPC marcher.</li> </ul></p> <p>`flip` specifies which planet to flip to. `False` means lower planet, `true` means upper planet.</p> <p>`immediate` specifies, if the player is targetted, whether to immediately move the camera to follow the player.</p> |
| `_activateMarbles`             | ---           | ---           | <p>Makes the other planets during `_zoomOutMultiplePlanets` and other zoom outs visible.</p> |
| `_deactivateMarbles`           | ---           | ---           | <p>Makes the other planets during `_zoomOutMultiplePlanets` and other zoom outs invisible.</p> |
| `_swapPlanet`                  | <p>`topPlanet`</p><p>`index`</p> | <p>Boolean</p><p>int</p> | <p>Swap the planet sprite for either of the zoomed in planets.</p> <p>`topPlanet` specifies which planet to swap. `False` means lower planet, `true` means upper planet.</p> <p>`index` specifies what planet sprite to swap the planet to. |
| `_marcherAnimation`            | <p>`which`</p><p>`animation`</p> | <p>int</p><p>Boolean</p> | <p>Makes the marchers play one of the count in animations.</p> <p>`which` specifies which marcher <ul><li>`-1` for all marchers.</li> <li>`0` for just the player.</li> <li>`1` for just the left NPC marcher.</li> <li>`2` for just the right NPC marcher.</li> </ul></p> <p>`animation` specifies what count in to animation play.<ul> <li>"Bop" plays the bop animation.</li> <li>"Ready" plays the animation leading into stepping.</li> </ul></p> |

**\*Events are already present in the vanilla game, just added to list of events in the mixtape editor.**
