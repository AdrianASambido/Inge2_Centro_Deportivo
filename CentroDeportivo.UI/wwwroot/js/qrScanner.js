window.escanearQR = async (videoId, dotNetRef) => {
    try {
        const video = document.getElementById(videoId);

        const stream = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: "environment"
            },
            audio: false
        });

        video.srcObject = stream;

        await video.play();

        // IMPORTANTE:
        // devolver true INMEDIATAMENTE
        // aunque ZXing falle después

        setTimeout(() => {
            iniciarScanner(video, dotNetRef);
        }, 500);

        return true;
    }
    catch (e) {
        console.error("ERROR CAMARA:", e);
        return false;
    }
};

window.iniciarScanner = function (video, dotNetRef) {

    const codeReader = new ZXing.BrowserQRCodeReader();

    codeReader.decodeFromVideoElement(video, (result, err) => {

        if (result) {
            dotNetRef.invokeMethodAsync(
                'ProcesarQrDetectado',
                result.text
            );
        }

        // IGNORAR errores normales
    });
};


window.detenerEscaner = function (videoId) {

    const video = document.getElementById(videoId);

    if (video && video.srcObject) {

        video.srcObject.getTracks().forEach(track => {
            track.stop();
        });

        video.srcObject = null;
    }
};
