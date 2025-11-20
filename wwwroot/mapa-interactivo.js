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

        // Variable para el marcador actual
        var currentMarker = null;

        // --- FUNCIÓN AUXILIAR: Mover marcador y avisar a Blazor ---
        // Esta función se usa tanto para los Clics como para el Buscador
        function actualizarMarcador(lat, lng) {
            // Si ya existe marcador, lo borramos
            if (currentMarker) {
                myMapInstance.removeLayer(currentMarker);
            }
            // Ponemos el nuevo
            currentMarker = L.marker([lat, lng]).addTo(myMapInstance);

            // Enviar coordenadas a C# (Formulario)
            // Usamos try-catch por si el componente de Blazor ya no existe
            try {
                dotNetHelper.invokeMethodAsync('SetMapCoordinates', lat, lng);
            } catch (err) {
                console.warn("No se pudo enviar coordenadas a Blazor:", err);
            }
        }

        // 6. --- INTEGRACIÓN DEL BUSCADOR (GEOCODER) ---
        // Verificamos si el plugin está cargado (por si acaso falló en index.html)
        if (L.Control.Geocoder) {
            var geocoder = L.Control.Geocoder.nominatim();

            L.Control.geocoder({
                geocoder: geocoder,
                defaultMarkGeocode: false, // Desactivamos el marcador por defecto del plugin para usar el nuestro
                placeholder: "Buscar ciudad o dirección...",
                errorMessage: "No encontrado"
            })
                .on('markgeocode', function (e) {
                    var result = e.geocode;

                    // 1. Centrar el mapa en el resultado encontrado
                    myMapInstance.fitBounds(result.bbox);

                    // 2. Usar nuestra función para poner el marcador y llenar el formulario
                    actualizarMarcador(result.center.lat, result.center.lng);
                })
                .addTo(myMapInstance);
        } else {
            console.warn("El plugin Leaflet-Control-Geocoder no está cargado. Revisa tu index.html.");
        }

        // 7. Manejo de Clics en el mapa (Manual)
        myMapInstance.on('click', function (e) {
            actualizarMarcador(e.latlng.lat, e.latlng.lng);
        });

        // 8. TRUCO FINAL: Forzar redibujado después de un momento
        setTimeout(function () {
            myMapInstance.invalidateSize();
        }, 300);

    } catch (error) {
        console.error("Error fatal al iniciar el mapa:", error);
    }
};