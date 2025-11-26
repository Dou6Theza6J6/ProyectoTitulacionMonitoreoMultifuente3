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

        // --- FUNCIÓN AUXILIAR: Mover marcador y BUSCAR DIRECCIÓN ---
        function actualizarMarcador(lat, lng) {
            // Si ya existe marcador, lo borramos
            if (currentMarker) {
                myMapInstance.removeLayer(currentMarker);
            }

            // Ponemos el nuevo (con draggable true para poder ajustarlo)
            currentMarker = L.marker([lat, lng], { draggable: true }).addTo(myMapInstance);

            // Evento: Si el usuario arrastra el marcador, actualizamos también
            currentMarker.on('dragend', function (e) {
                var position = e.target.getLatLng();
                buscarDireccionYEnviar(position.lat, position.lng);
            });

            // Llamamos a la búsqueda de dirección inmediatamente
            buscarDireccionYEnviar(lat, lng);
        }

        // --- NUEVA FUNCIÓN: CONECTA CON NOMINATIM Y ENVÍA A BLAZOR ---
        function buscarDireccionYEnviar(lat, lng) {
            var url = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&zoom=18&addressdetails=1`;

            fetch(url)
                .then(response => response.json())
                .then(data => {
                    var address = data.address || {};

                    // Extraer datos seguros
                    var calle = address.road || address.pedestrian || address.path || "";
                    var numero = address.house_number || "";
                    // Unimos calle y número
                    var direccionCompleta = (calle + " " + numero).trim();

                    var cp = address.postcode || "";
                    var ciudad = address.city || address.town || address.village || address.county || "";
                    var pais = address.country || "";

                    // ENVIAR A BLAZOR (Lat, Lng, Calle, CP, Ciudad, Pais)
                    try {
                        dotNetHelper.invokeMethodAsync('SetMapCoordinates', lat, lng, direccionCompleta, cp, ciudad, pais);
                    } catch (err) {
                        console.warn("No se pudo enviar a Blazor:", err);
                    }
                })
                .catch(error => {
                    console.error("Error obteniendo dirección:", error);
                    // Si falla la red, enviamos solo coordenadas
                    try {
                        dotNetHelper.invokeMethodAsync('SetMapCoordinates', lat, lng, "", "", "", "");
                    } catch (err) { }
                });
        }

        // 6. --- INTEGRACIÓN DEL BUSCADOR (GEOCODER) ---
        if (L.Control.Geocoder) {
            var geocoder = L.Control.Geocoder.nominatim();

            L.Control.geocoder({
                geocoder: geocoder,
                defaultMarkGeocode: false,
                placeholder: "Buscar ciudad o dirección...",
                errorMessage: "No encontrado"
            })
                .on('markgeocode', function (e) {
                    var result = e.geocode;
                    myMapInstance.fitBounds(result.bbox);
                    actualizarMarcador(result.center.lat, result.center.lng);
                })
                .addTo(myMapInstance);
        } else {
            console.warn("El plugin Leaflet-Control-Geocoder no está cargado.");
        }

        // 7. Manejo de Clics en el mapa (Manual)
        myMapInstance.on('click', function (e) {
            actualizarMarcador(e.latlng.lat, e.latlng.lng);
        });

        // 8. TRUCO FINAL: Forzar redibujado
        setTimeout(function () {
            myMapInstance.invalidateSize();
        }, 300);

    } catch (error) {
        console.error("Error fatal al iniciar el mapa:", error);
    }
};