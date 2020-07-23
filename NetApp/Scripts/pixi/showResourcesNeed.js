let serialRectangle = 0; // Порядковый номер проекта
let resText; // текст на панели с проектами
let RectanglePositions = [50, 0];
let textPosition = [25, 60];
const textureButton = PIXI.Texture.from('../Images/btnPlus.jpg');

const colors = [0xC0C0C0, 0xFFFF0B, 0xFF700B, 0x4286f4, 0x4286f4, 0xf441e8, 0x8dff6d, 0x41ccc9, 0xe03375, 0x95e032, 0x77c687, 0x43ba5b, 0x0ea3ba];
const style = new PIXI.TextStyle({
    fontSize: 12,
    wordWrap: true,
    wordWrapWidth: 40
});

const app = new PIXI.Application({ autoResize: true, resolution: devicePixelRatio, backgroundColor: 0xffffff });
document.getElementById("canvasCreateResource").appendChild(app.view);

window.addEventListener('resize', resize);
// Resize function window
function resize() {
    // Get the p
    const parent = app.view.parentNode;

    // Resize the renderer
    app.renderer.resize(parent.clientWidth, parent.clientHeight);

    // You can use the 'screen' property as the renderer visible
    // area, this is more useful than view.width/height because
    // it handles resolution
    //rect.position.set(app.screen.width, app.screen.height);
}

resize();

const graphics = new PIXI.Graphics();

const buttonPositions = [
    150, 25
];
const button = new PIXI.Sprite(textureButton);

button.anchor.set(0.5);
button.x = buttonPositions[0 * 2];
button.y = buttonPositions[0 * 2 + 1];

button.interactive = true;

button
    // Mouse & touch events are normalized into
    // the pointer* events for handling different
    // button events.
    .on('pointerdown', createResourse);

button.scale.set(0.09);
app.stage.addChild(button);


function createResourse() {
    window.top.postMessage("createResourseNeed", "*");
}