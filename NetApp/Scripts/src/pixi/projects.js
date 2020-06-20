//let widthWin = window.innerWidth; 
//let heightWin = window.innerHeight; 
let serialRectangle = 0; // Порядковый номер проекта
let resText; // текст на панели с проектами
let RectanglePositions = [50, 40];
let textPosition = [25, 60];
const textureButton = PIXI.Texture.from('../Images/btnPlus.jpg');

const colors = [0xC0C0C0, 0xFFFF0B, 0xFF700B, 0x4286f4, 0x4286f4, 0xf441e8, 0x8dff6d, 0x41ccc9, 0xe03375, 0x95e032, 0x77c687, 0x43ba5b, 0x0ea3ba];
const style = new PIXI.TextStyle({
    fontSize: 12,
    wordWrap: true,
    wordWrapWidth: 40
});
let projects = [];
class project {
    constructor(name, whatDoProject, whomProject, featureProject) {
        this.name = name;
        this.whatDoProject = whatDoProject;
        this.whomProject = whomProject;
        this.featureProject = featureProject;
    }
}

const app = new PIXI.Application({autoResize: true, resolution: devicePixelRatio,backgroundColor: 0xffffff});
//backgroundColor: 0xffffff
//let doc = document.getElementById('canvasProject');
document.body.appendChild(app.view);

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
button.x = buttonPositions[0*2];
button.y = buttonPositions[0 * 2 + 1];

button.interactive = true;

button
    // Mouse & touch events are normalized into
    // the pointer* events for handling different
    // button events.
    .on('pointerdown', createProject);

button.scale.set(0.09);
app.stage.addChild(button);


function createProject()
{
    let newWin = window.open("about:blank", "hello", "width=200,height=300");

    newWin.document.write(
        "<!DOCTYPE HTML><html><head><script src='../scripts/src/pixijs-legacy/node_modules/pixi.js-legacy/dist/pixi-legacy.js'></script></head><body><p>Название:</p><input type='text' id='nameProject'><p>Что делает:</p><input type='text' id='whatDoProject'><p>Кому полезен:</p><input type='text' id='whom'><p>В чем фишка:</p><input type='text' id='feature'><script src='../scripts/src/pixi/createProject.js'></script></body></html>"
    );
}

//function createRectangle(nameProject, whatDo, whom, feature)
//{
//    projects.push(new project(nameProject, whatDo, whom, feature));
//    idColor = 0;
//    graphics.beginFill(colors[idColor]);
//    if (serialRectangle % 3 != 0)
//    {
//        RectanglePositions[0] = 60 * (serialRectangle % 3) + 50;
//        textPosition[0] = 60 * (serialRectangle % 3) + 40;
//        //buttonRes.anchor.set(0.5);
//        //buttonRes.x = RectanglePositions[0];
//        //buttonRes.y = RectanglePositions[1];
//    }
//    else 
//    {
//        RectanglePositions[0] = 50;
//        RectanglePositions[1] += 80;
//        textPosition[0] = 40;
//        textPosition[1] += 80;
//    }

//    resText = new PIXI.Text(nameProject, style);
//    resText.x = textPosition[0];
//    resText.y = textPosition[1];

//    graphics.drawRect(RectanglePositions[0], RectanglePositions[1], 50, 50);
//    graphics.interactive = true;
//    graphics
//        // Mouse & touch events are normalized into
//        // the pointer* events for handling different
//        // button events.
//        .on('pointerdown', e => showDetailsProject(serialRectangle-1))

//    serialRectangle += 1;
//    graphics.endFill();
//    app.stage.addChild(graphics);
//    app.stage.addChild(resText);
//}

//function showDetailsProject(id) {
//    //alert(projects[id].name);

//    // Показать полупрозрачный DIV, чтобы затенить страницу
//    // (форма располагается не внутри него, а рядом, потому что она не должна быть полупрозрачной
//    let coverDiv = document.createElement('div');
//    coverDiv.id = 'cover-div';
//    //transform: translate(-50 %, -50 %);

//    // убираем возможность прокрутки страницы во время показа модального окна с формой
//    //document.body.style.overflowY = 'hidden';

//    document.body.append(coverDiv);

//    function hideCover() {
//        document.getElementById('cover-div').remove();
//        document.body.style.overflowY = '';
//    }

//   // function showPrompt(text, callback) {
//        //showCover();
//        let form = document.getElementById('prompt-form');
//        let container = document.getElementById('prompt-form-container');
//    let html = "<p>Название:</p><input type='text' id='Name' value=" + projects[id].name + "><p>Что делает:</p><input type='text' id='What' value=" + projects[id].whatDoProject + "><p>Кому полезен:</p><input type='text' id='Whom' value=" + projects[id].whomProject + "><p>В чем фишка:</p><input type='text' id='Feature' value=" + projects[id].featureProject+">";
//        document.getElementById('prompt-message').innerHTML = html;

//    //document.getElementById('nameProject').nodeValue = projects[id].name;
//    //document.getElementById('whatDoProject').nodeValue = projects[id].whatDoProject;
//    //document.getElementById('whom').nodeValue = projects[id].whomProject;
//    //document.getElementById('feature').nodeValue = projects[id].featureProject;
//        //form.text.value = '';

//        function complete(value) {
//            hideCover();
//            container.style.display = 'none';
//            document.onkeydown = null;
//            //callback(value);
//        }

//        //form.onsubmit = function () {
//        //    let value = form.text.value;
//        //    if (value == '') return false; // игнорируем отправку пустой формы

//        //    complete(value);
//        //    return false;
//        //};

//        //form.cancel.onclick = function () {
//        //    complete(null);
//        //};

//        document.onkeydown = function (e) {
//            if (e.key == 'Escape') {
//                complete(null);
//            }
//        };

//        //let lastElem = form.elements[form.elements.length - 1];
//        //let firstElem = form.elements[0];

//        //lastElem.onkeydown = function (e) {
//        //    if (e.key == 'Tab' && !e.shiftKey) {
//        //        firstElem.focus();
//        //        return false;
//        //    }
//        //};

//        //firstElem.onkeydown = function (e) {
//        //    if (e.key == 'Tab' && e.shiftKey) {
//        //        lastElem.focus();
//        //        return false;
//        //    }
//        //};

//        container.style.display = 'block';
//        form.elements.text.focus();
//    //}

//    //document.getElementById('show-button').onclick = function () {
//    //    showPrompt("Введите что-нибудь<br>...умное :)", function (value) {
//    //        alert("Вы ввели: " + value);
//    //    });
//    //};

//}