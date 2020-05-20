let width = window.innerWidth/2; //???????? ?????? ??????
let height = window.innerHeight / 2; // ???????? ?????? ??????
let buttons = [];
let addButton;
let choosedBtn;
let nameRes;
let tintBefor;
const style = new PIXI.TextStyle({ fontSize: 12 });
let disText = [
    new PIXI.Text("Финансы", style),
    new PIXI.Text("Люди", style),
    new PIXI.Text("Транспорт", style),
    new PIXI.Text("Оборудование", style)
];

//let serialRectangle = 0;
//let RectanglePositions = [50, 100];
//const colors = [0xFFFF0B, 0xFF700B, 0x4286f4, 0x4286f4, 0xf441e8, 0x8dff6d, 0x41ccc9, 0xe03375, 0x95e032, 0x77c687, 0x43ba5b, 0x0ea3ba];

const app = new PIXI.Application({ width:300, height:300, backgroundColor:0xffffff});
document.body.appendChild(app.view);

const graphics = new PIXI.Graphics();
//const outlineFilterBlue = new PIXI.filters.OutlineFilter(2, 0x99ff99);
const texturesButton = [
    PIXI.Texture.from('../Images/finance.png'),
    PIXI.Texture.from('../Images/people.png'),
    PIXI.Texture.from('../Images/transp.png'),
    PIXI.Texture.from('../Images/instrument.jpg'),
    PIXI.Texture.from('../Images/btnPlus.jpg')
];
const buttonPositions = [
    25, 25,
    80, 25,
    130, 25,
    25, 85,
    130, 130
];
const textPositions = [
    5, 50,
    60, 50,
    110, 50,
    5, 105
];

for (let i = 0; i < 4; i++) {
    const button = new PIXI.Sprite(texturesButton[i]);
    const text = disText[i];

    button.anchor.set(0.5);
    button.x = buttonPositions[i * 2];
    button.y = buttonPositions[i * 2 + 1];
    text.x = textPositions[i * 2];
    text.y = textPositions[i * 2 + 1];

    button.interactive = true;

    button
        // Mouse & touch events are normalized into
        // the pointer* events for handling different
        // button events.
        .on('pointerdown', (event) => filterOn(button))

    // add button to array
    buttons.push(button);

    app.stage.addChild(button);
    app.stage.addChild(text);

}

    buttons[0].scale.set(0.15);
    buttons[1].scale.set(0.05);
    buttons[2].scale.set(0.3);
    buttons[3].scale.set(0.4);

addButton = new PIXI.Sprite(texturesButton[4]);

addButton.anchor.set(0.5);
addButton.x = buttonPositions[4 * 2];
addButton.y = buttonPositions[4 * 2 + 1];

addButton.interactive = true;

addButton
    // Mouse & touch events are normalized into
    // the pointer* events for handling different
    // button events.
    .on('pointerdown', addResourse)
app.stage.addChild(addButton);
addButton.scale.set(0.07);


function filterOn(object) {
    tintBefor = object.tint;
    choosedBtn = object;

    for (let i = 0; i < buttons.length; i++)
    {
        buttons[i].tint = tintBefor;
    }
    object.tint = 0x4286f4;
}

function addResourse()
{
    nameRes = document.getElementById('nameResourse');
    choosedBtn.tint = tintBefor;
    window.opener.createRectangle(choosedBtn, nameRes.value);
    window.close();
}

function createRectangle()
{
   // RectanglePositions[0] = 50;
    idColor = 1;
    graphics.beginFill(colors[idColor]);
    if (serialRectangle == 3)
    {
        RectanglePositions[0] = 50;
        RectanglePositions[1] += 60;
        graphics.drawRect(RectanglePositions[0], RectanglePositions[1], 50, 50);
        serialRectangle = 0;
    }
    else
    {
        RectanglePositions[0] = 60 * serialRectangle + 50;
        graphics.drawRect(RectanglePositions[0], RectanglePositions[1], 50, 50);
        serialRectangle += 1;
    }
    graphics.endFill();

    app.stage.addChild(graphics);
}