//function hasGetUserMedia() {
//    // Note: Opera builds are unprefixed.
//    return !!(navigator.getUserMedia || navigator.webkitGetUserMedia ||
//        navigator.mozGetUserMedia || navigator.msGetUserMedia);
//}

//if (hasGetUserMedia()) {
//    // Good to go!
//} else {
//    alert('getUserMedia() is not supported in your browser');
//}

//let onFailSoHard = function (e) {
//    console.log('Reeeejected!', e);
//};

//let url = window.URL.createObjectURL(stream);

window.onload = function () {
    //let canvas = document.getElementById('canvas');
    let video = document.getElementById('video');
    //var button = document.getElementById('button');
    //var allow = document.getElementById('allow');
    //var context = canvas.getContext('2d');
    let videoStreamUrl = false;

    // функция которая будет выполнена при нажатии на кнопку захвата кадра
    //var captureMe = function () {
     //   if (!videoStreamUrl) alert('То-ли вы не нажали "разрешить" в верху окна, то-ли что-то не так с вашим видео стримом')
        // переворачиваем canvas зеркально по горизонтали (см. описание внизу статьи)
       // context.translate(canvas.width, 0);
        //context.scale(-1, 1);
        // отрисовываем на канвасе текущий кадр видео
        //context.drawImage(video, 0, 0, video.width, video.height);
        // получаем data: url изображения c canvas
        //var base64dataUrl = canvas.toDataURL('image/png');
        //context.setTransform(1, 0, 0, 1, 0, 0); // убираем все кастомные трансформации canvas
        // на этом этапе можно спокойно отправить  base64dataUrl на сервер и сохранить его там как файл (ну или типа того) 
        // но мы добавим эти тестовые снимки в наш пример:
        //var img = new Image();
        //img.src = base64dataUrl;
        //window.document.body.appendChild(img);
    //}

    //button.addEventListener('click', captureMe);

    // navigator.getUserMedia  и   window.URL.createObjectURL (смутные времена браузерных противоречий 2012)
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
    window.URL.createObjectURL = window.URL.createObjectURL || window.URL.webkitCreateObjectURL || window.URL.mozCreateObjectURL || window.URL.msCreateObjectURL;

    // запрашиваем разрешение на доступ к поточному видео камеры
    //navigator.getUserMedia({ video: true, audio: true }, function (stream) {
    //if (navigator.webkitGetUserMedia) {
    //    navigator.webkitGetUserMedia('audio, video', function (stream) {
    //        video.src = window.webkitURL.createObjectURL(stream);
    //    }, function () {
    //        console.log('что-то не так с видеостримом или пользователь запретил его использовать :P');
    //    });
    //}
    //else {
        navigator.getUserMedia({ video: true, audio: true }, function (stream) {
            // разрешение от пользователя получено
            // скрываем подсказку
            //allow.style.display = "none";
            // получаем url поточного видео
            videoStreamUrl = window.URL.createObjectURL(stream);
            // устанавливаем как источник для video 
            video.src = videoStreamUrl;
        }, function () {
            console.log('что-то не так с видеостримом или пользователь запретил его использовать :P');
        });
    //}
};
//// Not showing vendor prefixes.
//navigator.getUserMedia('video, audio', function (localMediaStream) {
//    var video = document.querySelector('video');
//    video.src = window.URL.createObjectURL(localMediaStream);

//    // Note: onloadedmetadata doesn't fire in Chrome when using it with getUserMedia.
//    // See crbug.com/110938.
//    video.onloadedmetadata = function (e) {
//        // Ready to go. Do some stuff.
//    };
//}, onFailSoHard);