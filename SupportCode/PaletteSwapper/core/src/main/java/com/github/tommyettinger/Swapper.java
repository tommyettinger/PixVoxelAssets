package com.github.tommyettinger;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.utils.ChangeListener;
import com.badlogic.gdx.utils.ObjectIntMap;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.kotcrab.vis.ui.VisUI;
import com.kotcrab.vis.ui.VisUI.SkinScale;
import com.kotcrab.vis.ui.util.dialog.Dialogs;
import com.kotcrab.vis.ui.widget.VisTable;
import com.kotcrab.vis.ui.widget.VisTextButton;
import com.kotcrab.vis.ui.widget.VisWindow;

/** {@link com.badlogic.gdx.ApplicationListener} implementation shared by all platforms. */
public class Swapper extends ApplicationAdapter {
    private Stage stage;
    private ObjectIntMap<String> paintColors, skinHairColors;
    String[] paintArray, skinHairArray;

    @Override
    public void create () {
        VisUI.load(SkinScale.X1);
        paintColors = new ObjectIntMap<String>(11);
        skinHairColors = new ObjectIntMap<String>(32);
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
                "Pale Skin, Blond Hair",
                "Pale Skin, Red Hair",
                "Pale Skin, Gray Hair",
                "Pale Skin, No Hair",
                "Light Skin, Brown Hair",
                "Light Skin, Black Hair",
                "Light Skin, Blond Hair",
                "Light Skin, Gray Hair",
                "Light Skin, No Hair",
                "Gold Skin, Black Hair",
                "Gold Skin, Gray Hair",
                "Gold Skin, No Hair",
                "Gold Skin, Scarlet Hair",
                "Gold Skin, Green Hair",
                "Gold Skin, Blue Hair",
                "Gold Skin, Magenta Hair",
                "Olive (Medium Brown) Skin, Black Hair",
                "Olive (Medium Brown) Skin, Gray Hair",
                "Olive (Medium Brown) Skin, No Hair",
                "Ochre (Warm Brown) Skin, Black Hair",
                "Ochre (Warm Brown) Skin, Gray Hair",
                "Ochre (Warm Brown) Skin, No Hair",
                "Coffee (Dark Brown) Skin, Black Hair",
                "Coffee (Dark Brown) Skin, Blond Hair",
                "Coffee (Dark Brown) Skin, Gray Hair",
                "Coffee (Dark Brown) Skin, No Hair",
        };
        for (int i = 0; i < skinHairArray.length; i++) {
            skinHairColors.put(skinHairArray[i], i);
        }
        stage = new Stage(new ScreenViewport());
        Gdx.input.setInputProcessor(stage);

        VisTable root = new VisTable();
        root.setFillParent(true);
        stage.addActor(root);

        final VisTextButton textButton = new VisTextButton("click me!");
        textButton.addListener(new ChangeListener() {
            @Override
            public void changed (ChangeEvent event, Actor actor) {
                textButton.setText("clicked");
                Dialogs.showOKDialog(stage, "message", "good job!");
            }
        });

        VisWindow window = new VisWindow("example window");
        window.add("this is a simple VisUI window").padTop(5f).row();
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