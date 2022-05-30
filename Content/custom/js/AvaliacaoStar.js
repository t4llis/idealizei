function Avaliar(estrelaSelecionada, idQuestao) {
   
    for (let i = 0; i <= 10; i++) {

        if (i <= estrelaSelecionada) {
            document.getElementById("star_" + i + "_" + idQuestao).src = "/images/star1.png";
            avaliacao = i + 1;
        } else {
            document.getElementById("star_" + i + "_" + idQuestao).src = "/images/star0.png";
            avaliacao = estrelaSelecionada;
        }            
           
    }

    if (avaliacao == 11) {
        avaliacao = 10;
    }
   
    $('#pontuacao_' + idQuestao).val(avaliacao);

}