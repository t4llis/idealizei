var Grafico = {};

Grafico.cor = function (valor) {
    if (valor <= 3) {
        return "#fe0000";
    }

    if (valor <= 4) {
        return "90-#fe0000:75-#ffa500:75-#ffa500";
    }

    if (valor <= 5) {
        return "90-#fe0000:60-#ffa500:60-#ffa500";
    }

    if (valor <= 6) {
        return "90-#fe0000:50-#ffa500:50-#ffa500";
    }

    if (valor <= 7) {
        return "90-#fe0000:42-#ffa500:42-#ffa500:84-#12ff00:84-#12ff00";
    }

    if (valor <= 8) {
        return "90-#fe0000:37-#ffa500:37-#ffa500:73-#12ff00:73-#12ff00";
    }

    if (valor <= 9) {
        return "90-#fe0000:32-#ffa500:32-#ffa500:66-#12ff00:66-#12ff00";
    }

    if (valor <= 10) {
        return "90-#fe0000:30-#ffa500:30-#ffa500:58-#12ff00:58-#12ff00"
    }

    return "90-#fe0000:30-#ffa500:30-#ffa500:58-#12ff00:58-#12ff00";
}