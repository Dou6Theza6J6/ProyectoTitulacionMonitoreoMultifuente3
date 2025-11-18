let map;
let marker;

// Esta función es llamada desde la página DatosMuestra.razor
window.initMap = (dotNetHelper) => {
    // Si el mapa ya existe, no hagas nada para evitar duplicados
    if (map) return;

    // Coordenadas iniciales (Península de Yucatán)
    map = L.map('map').setView([20.7, -88.9], 8);

    // Añade la capa de OpenStreetMap (el mapa visual)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    // Evento que se dispara cuando el usuario hace clic en el mapa
    map.on('click', function (e) {
        const lat = e.latlng.lat;
        const lng = e.latlng.lng;

        if (marker) {
            map.removeLayer(marker);
        }
        marker = L.marker([lat, lng]).addTo(map);

        // Llama a la función de C# en DatosMuestra.razor para enviarle las coordenadas
        dotNetHelper.invokeMethodAsync('SetMapCoordinates', lat, lng);
    });
};