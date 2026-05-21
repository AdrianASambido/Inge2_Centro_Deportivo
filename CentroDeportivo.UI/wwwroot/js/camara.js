window.qrReader = null;

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

        window.qrReader = new ZXing.BrowserQRCodeReader();

        window.qrReader.decodeFromVideoDevice(
            null,
            videoId,
            (result, err) => {

                if (result) {

                    console.log("QR:", result.text);

                    dotNetRef.invokeMethodAsync(
                        "ProcesarQrDetectado",
                        result.text
                    );
                }

                if (err && !(err instanceof ZXing.NotFoundException)) {
                    console.error(err);
                }
            }
        );

        return true;
    }
    catch (e) {

        console.error("ERROR CAMARA:", e);

        return false;
    }
};

window.detenerEscaner = async function (videoId) {

    if (window.qrReader) {

        await window.qrReader.reset();

        window.qrReader = null;
    }

    const video = document.getElementById(videoId);

    if (video && video.srcObject) {

        video.srcObject.getTracks().forEach(track => {
            track.stop();
        });

        video.srcObject = null;
    }
};