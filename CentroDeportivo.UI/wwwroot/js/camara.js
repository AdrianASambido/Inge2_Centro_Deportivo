// Usamos una variable global para mantener la referencia al stream
let streamPrincipal = null;

window.iniciarCamara = async function (videoId) {
    try {
        const video = document.getElementById(videoId);
        if (!video) {
            console.error("No se encontró el elemento video con ID:", videoId);
            return false;
        }

        // Si ya hay una cámara encendida, la detenemos primero para evitar conflictos
        if (streamPrincipal) {
            streamPrincipal.getTracks().forEach(track => track.stop());
        }

        // Solicitamos acceso a la cámara
        streamPrincipal = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: "environment", // Prioriza cámara trasera en móviles
                width: { ideal: 1280 },    // Pedimos buena resolución para que el QR se lea bien
                height: { ideal: 720 }
            }
        });

        video.srcObject = streamPrincipal;

        // Esperamos a que el video esté listo para reproducirse
        return new Promise((resolve) => {
            video.onloadedmetadata = () => {
                video.play();
                resolve(true);
            };
        });

    } catch (err) {
        console.error("Error al acceder a la cámara:", err);
        return false;
    }
};

window.capturarFoto = async function (videoId) {
    try {
        const video = document.getElementById(videoId);
        // Si el video no está listo o no hay stream, salimos
        if (!video || !streamPrincipal || video.readyState !== 4) return null;

        const canvas = document.createElement("canvas");
        // Capturamos a la resolución real del video para no perder calidad en el QR
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        const ctx = canvas.getContext("2d");
        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

        // Subimos un poquito la calidad (0.9) para que ZXing no sufra con los píxeles
        const base64 = canvas.toDataURL("image/jpeg", 0.9);

        // Enviamos solo el contenido Base64 puro a C#
        return base64.split(",")[1];
    } catch (err) {
        console.error("Error al capturar foto:", err);
        return null;
    }
};

window.detenerCamara = function () {
    if (streamPrincipal) {
        streamPrincipal.getTracks().forEach(track => track.stop());
        streamPrincipal = null;
        console.log("Cámara detenida y stream liberado.");
    }
};