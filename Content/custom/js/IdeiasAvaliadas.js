$(document).ready(function () {

    let urlAtual = window.location.href;
    if (urlAtual.includes($URL_PRINCIPAL + 'Avaliacao')) {

        CarregaInfoCardsIdeiasAvaliadas();
        CarregaInfoCardsIdeiasAvaliadasFeedback();

        $('.btn-auxiliar-grafico').on("click", function () {
            CarregaInfoCardsIdeiasAvaliadasFeedbackByIdIdeia()
        });

        let ItensCarrocel = document.querySelector('.carrocel-comentarios-itens');
        if (ItensCarrocel != null) {
            new Glider(ItensCarrocel, {
                slidesToShow: 1,
                slidesToScroll: 1,
                draggable: true,
                arrows: {
                    prev: '.glider-prev',
                    next: '.glider-next'
                }
            });
        }
    }

});

function CarregaInfoCardsIdeiasAvaliadas() {
    Loading.start();
    let ObjTotais = '{';
    ObjTotais += '"AVALIACAOENUM":"AVALIADOR"';
    ObjTotais += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Avaliacao/BuscarInfoCardsIdeiasAvaliadas',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                let index = 0;
                while (index < data.length)
                {
                    if ($('#graph_bar_' + data[index].ID).length) {
                        Morris.Bar({
                            element: 'graph_bar_' + data[index].ID,
                            data: [
                                {
                                    device: data[index].LETRA,
                                    geekbench: (data[index].PONTUACAO / data[index].QTD_QUESTOES_AVALIACAO).toFixed(2),
                                    legenda: data[index].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 1].LETRA,
                                    geekbench: (data[index + 1].PONTUACAO / data[index + 1].QTD_QUESTOES_AVALIACAO).toFixed(2),
                                    legenda: data[index + 1].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 2].LETRA,
                                    geekbench: (data[index + 2].PONTUACAO / data[index + 2].QTD_QUESTOES_AVALIACAO).toFixed(2),
                                    legenda: data[index + 2].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 3].LETRA,
                                    geekbench: (data[index + 3].PONTUACAO / data[index + 3].QTD_QUESTOES_AVALIACAO).toFixed(2),
                                    legenda: data[index + 3].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 4].LETRA,
                                    geekbench: (data[index + 4].PONTUACAO / data[index + 4].QTD_QUESTOES_AVALIACAO).toFixed(2),
                                    legenda: data[index + 4].LEGENDA_GRAFICO
                                }
                            ],
                            ymax: 10,
                            xkey: 'device',
                            ykeys: ['geekbench'],
                            labels: ['Pontuação'],
                            barRatio: 0.4,
                            barColors: function (row, series, type) {
                                return Grafico.cor(row.y);
                            },                            
                            hideHover: 'auto',
                            hoverCallback: function (index, options, content, row) {
                                var hover = "<div class='morris-hover-row-label'>" + row.legenda + "</div> " +
                                    " <div class='morris-hover-point' > " +
                                    "<p style='font-size: 20px';> " +
                                    " Pontuação: " + row.geekbench +
                                    "</p > " +
                                    "</div > ";
                                return hover;
                            },
                            resize: true
                        });

                    }
                    index = index + 5;
                }
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function CarregaInfoCardsIdeiasAvaliadasFeedback() {
    Loading.start();
    let ObjTotais = '{';    
    ObjTotais += '"AVALIACAOENUM":"FEEDBACK"';    
    ObjTotais += '}';    

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Avaliacao/BuscarInfoCardsIdeiasAvaliadas',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                let index = 0;
                while (index < data.length) {

                    let quantidadeFeedbacks = $('#id_quantidade_feedbacks_' + data[index].ID).html();

                    if ($('#graph_bar_feedback_' + data[index].ID).length) {

                        Morris.Bar({
                            element: 'graph_bar_feedback_' + data[index].ID,
                            data: [
                                {
                                    device: data[index].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index].PONTUACAO, parseInt(quantidadeFeedbacks), data[index].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 1].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 1].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 1].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 1].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 2].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 2].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 2].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 2].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 3].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 3].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 3].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 3].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 4].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 4].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 4].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 4].LEGENDA_GRAFICO
                                }
                            ],
                            ymax: 10,
                            xkey: 'device',
                            ykeys: ['geekbench'],
                            labels: ['Pontuação'],
                            barRatio: 0.4,
                            barColors: function (row, series, type) {
                                return Grafico.cor(row.y);
                            },                            
                            hideHover: 'auto',
                            hoverCallback: function (index, options, content, row) {
                                var hover = "<div class='morris-hover-row-label'>" + row.legenda + "</div> " +
                                    " <div class='morris-hover-point' > " +
                                    "<p style='font-size: 20px';> " +
                                    " Pontuação: " + row.geekbench +
                                    "</p > " +
                                    "</div > ";
                                return hover;
                            },
                            resize: true
                        });

                    }
                    index = index + 5;
                }
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function CarregaInfoCardsIdeiasAvaliadasFeedbackByIdIdeia() {
    Loading.start();
    let idIdeia = $('.btn-auxiliar-grafico').val();

    let ObjTotais = '{';
    ObjTotais += '"AVALIACAOENUM":"FEEDBACK",';
    ObjTotais += '"IDIDEIA":"' + idIdeia + '"';
    ObjTotais += '}';

    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Avaliacao/BuscarInfoCardsIdeiasAvaliadas',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                let index = 0;
                while (index < data.length) {

                    let quantidadeFeedbacks = $('#qntFeedbacksRecebidos').html();

                    if ($('#graph_bar_feedback_ideia_' + data[index].ID).length) {

                        Morris.Bar({
                            element: 'graph_bar_feedback_ideia_' + data[index].ID,
                            data: [
                                {
                                    device: data[index].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index].PONTUACAO, parseInt(quantidadeFeedbacks), data[index].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 1].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 1].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 1].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 1].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 2].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 2].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 2].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 2].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 3].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 3].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 3].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 3].LEGENDA_GRAFICO
                                },
                                {
                                    device: data[index + 4].LETRA,
                                    geekbench: calculaMediaPontuacao(data[index + 4].PONTUACAO, parseInt(quantidadeFeedbacks), data[index + 4].QTD_QUESTOES_AVALIACAO),
                                    legenda: data[index + 4].LEGENDA_GRAFICO
                                }
                            ],
                            ymax: 10,
                            xkey: 'device',
                            ykeys: ['geekbench'],
                            labels: ['Pontuação'],
                            barRatio: 0.4,
                            barColors: function (row, series, type) {
                                return Grafico.cor(row.y);
                            },                            
                            hideHover: 'auto',
                            hoverCallback: function (index, options, content, row) {
                                var hover = "<div class='morris-hover-row-label'>" + row.legenda + "</div> " +
                                    " <div class='morris-hover-point' > " +
                                    "<p style='font-size: 20px';> " +
                                    " Pontuação: " + row.geekbench +
                                    "</p > " +
                                    "</div > ";
                                return hover;
                            },
                            resize: true
                        });

                    }
                    index = index + 5;
                }
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

$('.btn-modal-observacao').on("click", function () {
    $('.observacao_avaliador').val('');

    let idUsuarioAvaliador = $(this).attr("data-idusuarioavaliador");

    comentariosAvaliacao.forEach(function (comentarios) {
        if (comentarios.ID_USUARIO_AVALIADOR == idUsuarioAvaliador) {
            $('.observacao_avaliador').val(comentarios.OBSERVACAO);
        }
    });
});

function calculaMediaPontuacao(pontuacao, quantidadeFeedbacks, qtdQuestoesAvaliacao) {
    qtdQuestoesAvaliacao = qtdQuestoesAvaliacao / quantidadeFeedbacks;

    return (pontuacao / (quantidadeFeedbacks * qtdQuestoesAvaliacao)).toFixed(2);
}