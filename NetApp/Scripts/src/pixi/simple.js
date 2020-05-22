//import * as PIXI from '../pixi.js-legacy';
let width = window.innerWidth/2; //???????? ?????? ??????
let height = window.innerHeight/2; // ???????? ?????? ??????
let serialRectangle = 0;
let resText;
let RectanglePositions = [50, 150];
let textPosition = [25, 170];
const colors = [0xFFFF0B, 0xFF700B, 0x4286f4, 0x4286f4, 0xf441e8, 0x8dff6d, 0x41ccc9, 0xe03375, 0x95e032, 0x77c687, 0x43ba5b, 0x0ea3ba];
const style = new PIXI.TextStyle({
    fontSize: 12,
    wordWrap: true,
    wordWrapWidth: 40
});
//const style = new PIXI.TextStyle({
//    fontFamily: 'Arial',
//    fontSize: 36,
//    fontStyle: 'italic',
//    fontWeight: 'bold',
//    fill: ['#ffffff', '#00ff99'], // gradient
//    stroke: '#4a1850',
//    strokeThickness: 5,
//    dropShadow: true,
//    dropShadowColor: '#000000',
//    dropShadowBlur: 4,
//    dropShadowAngle: Math.PI / 6,
//    dropShadowDistance: 6,
//    wordWrap: true,
//    wordWrapWidth: 440,
//});
const app = new PIXI.Application({ backgroundColor: 0xffffff });
document.body.appendChild(app.view);

const graphics = new PIXI.Graphics();
const textureButton = PIXI.Texture.from('../Images/btnPlus.jpg');
const buttonPositions = [
    175, 75
];
const button = new PIXI.Sprite(textureButton);

button.anchor.set(0.5);
button.x = buttonPositions[0*2];
button.y = buttonPositions[0 * 2 + 1];

button.interactive = true;

button
    // Mouse & touch events are normalized into
    // the pointer* events for handling different
    // button events.
    .on('pointerdown', createResourse)

button.scale.set(0.1);
app.stage.addChild(button);


function createResourse()
{
    let newWin = window.open("about:blank", "hello", "width=200,height=300");
    let hostName = "https://localhost:44353";
    let adressPixi = hostName + "/scripts/src / pixijs - legacy / node_modules / pixi.js - legacy / dist / pixi - legacy.js";
    let adressJS = hostName + "/scripts/src/pixi/createResourse.js";
    let html = "<!DOCTYPE HTML><html><head><script src='" + adressPixi + "'></script></head><body><p>Ресурс:</p><input type='text' id='nameResourse'><script src='" + adressJS + "'></script></body></html>";
    newWin.document.write(
        "<!DOCTYPE HTML><html><head><script src='../scripts/src/pixijs-legacy/node_modules/pixi.js-legacy/dist/pixi-legacy.js'></script></head><body><p>Ресурс:</p><input type='text' id='nameResourse'><script src='../scripts/src/pixi/createResourse.js'></script></body></html>"
        //html
        //"<!DOCTYPE HTML><html><body><iframe srcdoc=<script src='https://localhost:44353/scripts/src/pixijs-legacy/node_modules/pixi.js-legacy/dist/pixi-legacy.js'></script><script src='https://localhost:44353/scripts/src/pixi/createResourse.js'></script></iframe></body></html>"
    );
}

function createRectangle(buttonRes, nameRes)
{
   // RectanglePositions[0] = 50;
    //idColor = 1;
    //graphics.beginFill(colors[idColor]);
    if (serialRectangle == 3)
    {
        RectanglePositions[0] = 50;
        RectanglePositions[1] += 70;
        textPosition[0] = 25;
        textPosition[1] += 70;
        //graphics.drawRect(RectanglePositions[0], RectanglePositions[1], 50, 50);
        buttonRes.anchor.set(0.5);
        buttonRes.x = RectanglePositions[0];
        buttonRes.y = RectanglePositions[1];

        resText = new PIXI.Text(nameRes, style);
        //resText = new PIXI.Text("финансы", style);
        resText.x = textPosition[0];
        resText.y = textPosition[1];

        serialRectangle = 0;
    }
    else
    {
        RectanglePositions[0] = 60 * serialRectangle + 50;
        textPosition[0] = 60 * serialRectangle + 25;
        buttonRes.anchor.set(0.5);
        buttonRes.x = RectanglePositions[0];
        buttonRes.y = RectanglePositions[1];

        resText = new PIXI.Text(nameRes, style);
        //RectanglePositions[0] = 60 * serialRectangle + 50;
        //resText = new PIXI.Text("финансы", style);
        resText.x = textPosition[0];
        resText.y = textPosition[1];
        
        //graphics.drawRect(RectanglePositions[0], RectanglePositions[1], 50, 50);
        serialRectangle += 1;
    }
    //graphics.endFill();
    app.stage.addChild(buttonRes);
    app.stage.addChild(resText);
}