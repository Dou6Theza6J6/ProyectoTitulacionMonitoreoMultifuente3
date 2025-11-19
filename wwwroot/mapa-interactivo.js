var myMapInstance = null; // Usamos un nombre distinto para evitar conflictos globales

window.initMap = (dotNetHelper) => {
    // 1. Verificar que la librería Leaflet (L) se haya cargado
    if (typeof L === 'undefined') {
        console.error("Leaflet no se cargó correctamente. Verifica tu conexión a internet o el enlace en index.html");
        return;
    }

    // 2. Verificar que el contenedor del mapa exista en el HTML
    var container = document.getElementById('map');
    if (!container) {
        console.error("No se encontró el div con id='map'");
        return;
    }

    // 3. Limpieza agresiva: Si ya había un mapa, intentar borrarlo suavemente
    if (myMapInstance) {
        try {
            myMapInstance.off(); // Desconectar eventos
            myMapInstance.remove(); // Borrar mapa de Leaflet
        } catch (e) {
            console.warn("Error limpiando mapa anterior (es normal al cambiar de página):", e);
        }
        myMapInstance = null;
    }

    // 4. Limpieza manual: Asegurar que el div esté vacío (elimina residuos viejos)
    container.innerHTML = "";

    try {
        // 5. Crear el mapa nuevo
        myMapInstance = L.map('map').setView([20.7, -88.9], 8);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(myMapInstance);

        // 6. Manejo de Marcadores y Clics
        var currentMarker = null;
        myMapInstance.on('click', function (e) {
            var lat = e.latlng.lat;
            var lng = e.latlng.lng;

            if (currentMarker) {
                myMapInstance.removeLayer(currentMarker);
            }
            currentMarker = L.marker([lat, lng]).addTo(myMapInstance);

            // Enviar coordenadas a C#
            dotNetHelper.invokeMethodAsync('SetMapCoordinates', lat, lng);
        });

        // 7. TRUCO FINAL: Forzar redibujado después de un momento
        // Esto arregla el "mapa gris" o "incompleto" que ocurre en Blazor al renderizar
        setTimeout(function () {
            myMapInstance.invalidateSize();
        }, 300);

    } catch (error) {
        console.error("Error fatal al iniciar el mapa:", error);
    }

};
