package com.github.tommyettinger;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.TextureAtlas;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.utils.ChangeListener;
import com.badlogic.gdx.utils.ObjectIntMap;
import com.badlogic.gdx.utils.OrderedMap;
import com.badlogic.gdx.utils.OrderedSet;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.kotcrab.vis.ui.VisUI;
import com.kotcrab.vis.ui.VisUI.SkinScale;
import com.kotcrab.vis.ui.util.dialog.Dialogs;
import com.kotcrab.vis.ui.widget.VisCheckBox;
import com.kotcrab.vis.ui.widget.VisTable;
import com.kotcrab.vis.ui.widget.VisTextButton;
import com.kotcrab.vis.ui.widget.VisWindow;

/** {@link com.badlogic.gdx.ApplicationListener} implementation shared by all platforms. */
public class Swapper extends ApplicationAdapter {
    private Stage stage;
    private ObjectIntMap<String> paintColors, skinHairColors, weaponMapping;
    private OrderedMap<String, String> unitMapping;
    private String[] paintArray, skinHairArray, unitArray;
    private OrderedSet<String> femaleUnits;

    @Override
    public void create () {
        VisUI.load(SkinScale.X1);
        paintColors = new ObjectIntMap<String>(11);
        skinHairColors = new ObjectIntMap<String>(32);
        weaponMapping = new ObjectIntMap<String>(64);
        paintArray = new String[]{
                "Dark",
                "White",
                "Red",
                "Orange",
                "Yellow",
                "Green",
                "Blue",
                "Purple",
        };
        for (int i = 0; i < 8; i++) {
            paintColors.put(paintArray[i], i);
        }
        skinHairArray = new String[]{
                "Pale Skin\nBlond Hair",
                "Pale Skin\nRed Hair",
                "Pale Skin\nGray Hair",
                "Pale Skin\nNo Hair",
                "Light Skin\nBrown Hair",
                "Light Skin\nBlack Hair",
                "Light Skin\nBlond Hair",
                "Light Skin\nGray Hair",
                "Light Skin\nNo Hair",
                "Gold Skin\nBlack Hair",
                "Gold Skin\nGray Hair",
                "Gold Skin\nNo Hair",
                "Gold Skin\nScarlet Hair",
                "Gold Skin\nGreen Hair",
                "Gold Skin\nBlue Hair",
                "Gold Skin\nMagenta Hair",
                "Mid Brown Skin\nBlack Hair",
                "Mid Brown Skin\nGray Hair",
                "Mid Brown Skin\nNo Hair",
                "Warm Brown Skin\nBlack Hair",
                "Warm Brown Skin\nGray Hair",
                "Warm Brown Skin\nNo Hair",
                "Dark Brown Skin\nBlack Hair",
                "Dark Brown Skin\nBlond Hair",
                "Dark Brown Skin\nGray Hair",
                "Dark Brown Skin\nNo Hair",
        };
        for (int i = 0; i < skinHairArray.length; i++) {
            skinHairColors.put(skinHairArray[i], i);
        }
        unitArray = new String[]{
                "Infantry",
                "Bazooka",
                "Bike",
                "Rifle Sniper",
                "Missile Sniper",
                "Mortar Sniper",
                "Light Tank",
                "War Tank",
                "Heavy Cannon",
                "Light Artillery",
                "AA Artillery",
                "Stealth Artillery",
                "Recon",
                "AA Gun",
                "Flamethrower",
                "Prop Plane",
                "Heavy Bomber",
                "Fighter Jet",
                "Supply Truck",
                "Amphi Transport",
                "Transport Copter",
                "Jetpack",
                "Gunship Copter",
                "Blitz Copter",
                "Build Rig",
                "Jammer",
                "Comm Copter",
                "Mud Tank",
                "Submarine",
                "Stealth Jet",
                "Patrol Boat",
                "Cruiser",
                "Battleship",
                "Volunteer",
                "Engineer",
                "Smuggler",
                "Medic",
                "Civilian"
        };
        femaleUnits = new OrderedSet<String>(32);
        femaleUnits.addAll(
                "Infantry",
                "Infantry_P",
                "Infantry_S",
                "Infantry_T",
                "Infantry_PS",
                "Infantry_PT",
                "Infantry_ST",
                "Artillery",
                "Artillery_P",
                "Artillery_S",
                "Artillery_T",
                "Tank",
                "Tank_P",
                "Tank_S",
                "Tank_T",
                "Truck_P",
                "Recon",
                "Flamethrower",
                "Volunteer",
                "Volunteer_P",
                "Volunteer_S",
                "Volunteer_T",
                "Civilian"
        );
        unitMapping = new OrderedMap<String, String>(64);
        unitMapping.put("Infantry", "Infantry");
        unitMapping.put("Bazooka", "Infantry_P");
        unitMapping.put("Bike", "Infantry_S");
        unitMapping.put("Rifle Sniper", "Infantry_T");
        unitMapping.put("Missile Sniper", "Infantry_PS");
        unitMapping.put("Mortar Sniper", "Infantry_PT");
        unitMapping.put("Light Tank", "Tank");
        unitMapping.put("War Tank", "Tank_P");
        unitMapping.put("Heavy Cannon", "Artillery_P");
        unitMapping.put("Light Artillery", "Artillery");
        unitMapping.put("AA Artillery", "Artillery_S");
        unitMapping.put("Stealth Artillery", "Artillery_T");
        unitMapping.put("Recon", "Recon");
        unitMapping.put("AA Gun", "Tank_S");
        unitMapping.put("Flamethrower", "Flamethrower");
        unitMapping.put("Prop Plane", "Plane");
        unitMapping.put("Heavy Bomber", "Plane_P");
        unitMapping.put("Fighter Jet", "Plane_S");
        unitMapping.put("Supply Truck", "Truck");
        unitMapping.put("Amphibious Transport", "Truck_S");
        unitMapping.put("Transport Copter", "Copter");
        unitMapping.put("Jetpack", "Infantry_ST");
        unitMapping.put("Gunship Copter", "Copter_P");
        unitMapping.put("Blitz Copter", "Copter_S");
        unitMapping.put("Build Rig", "Truck_T");
        unitMapping.put("Jammer", "Truck_P");
        unitMapping.put("Comm Copter", "Copter_T");
        unitMapping.put("Mud Tank", "Tank_T");
        unitMapping.put("Submarine", "Boat_T");
        unitMapping.put("Stealth Jet", "Plane_T");
        unitMapping.put("Patrol Boat", "Boat");
        unitMapping.put("Cruiser", "Boat_S");
        unitMapping.put("Battleship", "Boat_P");
        unitMapping.put("Volunteer", "Volunteer");
        unitMapping.put("Engineer", "Volunteer_P");
        unitMapping.put("Smuggler", "Volunteer_S");
        unitMapping.put("Medic", "Volunteer_T");
        unitMapping.put("Civilian", "Civilian");

        weaponMapping.put("Infantry", 1);
        weaponMapping.put("Infantry_P", 3);
        weaponMapping.put("Infantry_S", 1);
        weaponMapping.put("Infantry_T", 3);
        weaponMapping.put("Infantry_PS", 1);
        weaponMapping.put("Infantry_PT", 2);
        weaponMapping.put("Infantry_ST", 1);
        weaponMapping.put("Artillery", 2);
        weaponMapping.put("Artillery_P", 1);
        weaponMapping.put("Artillery_S", 2);
        weaponMapping.put("Artillery_T", 2);
        weaponMapping.put("Tank", 3);
        weaponMapping.put("Tank_P", 3);
        weaponMapping.put("Tank_S", 1);
        weaponMapping.put("Tank_T", 3);
        weaponMapping.put("Plane", 1);
        weaponMapping.put("Plane_P", 2);
        weaponMapping.put("Plane_S", 1);
        weaponMapping.put("Plane_T", 1);
        weaponMapping.put("Truck", 0);
        weaponMapping.put("Truck_P", 0);
        weaponMapping.put("Truck_S", 0);
        weaponMapping.put("Truck_T", 0);
        weaponMapping.put("Copter", 0);
        weaponMapping.put("Copter_P", 3);
        weaponMapping.put("Copter_S", 1);
        weaponMapping.put("Copter_T", 0);
        weaponMapping.put("Boat", 1);
        weaponMapping.put("Boat_P", 1);
        weaponMapping.put("Boat_S", 3);
        weaponMapping.put("Boat_T", 3);
        weaponMapping.put("Recon", 1);
        weaponMapping.put("Flamethrower", 1);
        weaponMapping.put("City", 0);
        weaponMapping.put("Factory", 0);
        weaponMapping.put("Airport", 0);
        weaponMapping.put("Dock", 0);
        weaponMapping.put("Laboratory", 0);
        weaponMapping.put("Castle", 0);
        weaponMapping.put("Estate", 0);
        weaponMapping.put("Volunteer", 0);
        weaponMapping.put("Volunteer_P", 0);
        weaponMapping.put("Volunteer_S", 0);
        weaponMapping.put("Volunteer_T", 0);
        weaponMapping.put("Civilian", 0);

        TextureAtlas atlas = new TextureAtlas("WargameAbove.atlas");
        Texture palette = new Texture("palettes/default.png");
        stage = new Stage(new ScreenViewport());
        Gdx.input.setInputProcessor(stage);

        VisTable root = new VisTable();
        root.setFillParent(true);
        stage.addActor(root);
        VisWindow window = new VisWindow("Palette Swapper");
        window.add("Paint and Uniform Colors:").pad(5f).row();;
        final VisCheckBox[] paintBoxes = new VisCheckBox[8], skinBoxes = new VisCheckBox[skinHairArray.length],
        unitBoxes = new VisCheckBox[unitArray.length];
        for (int i = 0; i < 8; i++) {
            window.add(paintBoxes[i] = new VisCheckBox(paintArray[i] + " Paint", i < 2)).pad(5f);
        }
        window.row();
        window.add("Skin and Hair Colors:").pad(5f).row();
        for (int i = 0; i < skinHairArray.length; i++) {
            window.add(skinBoxes[i] = new VisCheckBox(skinHairArray[i], true)).pad(5f);
            if(i % 8 == 7)
                window.row();
        }
        window.row();
        window.add("Unit Types:").pad(5f).row();
        for (int i = 0; i < unitArray.length; i++) {
            window.add(unitBoxes[i] = new VisCheckBox(unitArray[i], i == 0)).pad(5f);
            if(i % 8 == 7)
                window.row();
        }
        window.row();
        VisCheckBox female = new VisCheckBox("Generate Female Units?", true);
        VisCheckBox receiving = new VisCheckBox("Generate 'receiving\nattack' animations?", false);
        VisCheckBox environment = new VisCheckBox("Generate flat\nenvironment tiles?", false);
        VisCheckBox slopes = new VisCheckBox("Generate sloped\nenvironment tiles?", false);
        window.add(female);
        window.add(receiving);
        window.add(environment);
        window.add(slopes);
        window.row();
        final VisTextButton textButton = new VisTextButton("Generate!");
        textButton.addListener(new ChangeListener() {
            @Override
            public void changed (ChangeEvent event, Actor actor) {
                Dialogs.showOKDialog(stage, "Sorry!", "Not implemented just yet!");
            }
        });

        window.add(textButton).pad(10f);
        window.pack();
        window.centerWindow();
        stage.addActor(window.fadeIn());
    }

    @Override
    public void resize (int width, int height) {
        stage.getViewport().update(width, height, true);
    }

    @Override
    public void render () {
        Gdx.gl.glClearColor(0f, 0f, 0f, 1f);
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
        stage.act(Math.min(Gdx.graphics.getDeltaTime(), 1 / 30f));
        stage.draw();
    }

    @Override
    public void dispose () {
        VisUI.dispose();
        stage.dispose();
    }
}