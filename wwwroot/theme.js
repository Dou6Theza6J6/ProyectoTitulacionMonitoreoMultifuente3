// wwwroot/theme.js

window.ThemeManager = {
    // Aplica el tema seleccionado
    setTheme: function (theme) {
        if (theme === 'auto') {
            // Si es auto, pregunta al sistema operativo
            if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
                document.documentElement.setAttribute('data-bs-theme', 'dark');
            } else {
                document.documentElement.setAttribute('data-bs-theme', 'light');
            }
        } else {
            // Si es manual (dark/light)
            document.documentElement.setAttribute('data-bs-theme', theme);
        }
        // Guardar preferencia
        localStorage.setItem('preferred-theme', theme);
    },

    // Lee la preferencia guardada al iniciar
    getSavedTheme: function () {
        return localStorage.getItem('preferred-theme') || 'auto';
    },

    // Inicializa al arrancar la app
    initTheme: function () {
        const savedTheme = this.getSavedTheme();
        this.setTheme(savedTheme);
    }
};

// Ejecutar inmediatamente para evitar "parpadeo" blanco al cargar
window.ThemeManager.initTheme();