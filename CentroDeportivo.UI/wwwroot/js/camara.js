let stream = null;

window.iniciarCamara = async function (videoId) {
    try {
        const video = document.getElementById(videoId);
        if (!video) {
            console.error("No se encontró el elemento video con ID:", videoId);
            return false;
        }

        stream = await navigator.mediaDevices.getUserMedia({
            video: { facingMode: "environment" } // cámara trasera en móvil
        });

        video.srcObject = stream;
        await video.play();
        return true;
    } catch (err) {
        console.error("Error al acceder a la cámara:", err);
        return false;
    }
};

window.capturarFoto = async function (videoId) {
    try {
        const video = document.getElementById(videoId);
        if (!video || !stream) return null;

        const canvas = document.createElement("canvas");
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        const ctx = canvas.getContext("2d");
        ctx.drawImage(video, 0, 0);

        // Devuelve la imagen en base64 sin el prefijo
        const base64 = canvas.toDataURL("image/jpeg", 0.8);
        return base64.split(",")[1];
    } catch (err) {
        console.error("Error al capturar foto:", err);
        return null;
    }
};

window.detenerCamara = function () {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
        stream = null;
    }
};