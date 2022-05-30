$(document).ready(function () {

    let urlAtual = window.location.href;
    if (urlAtual.includes($URL_PRINCIPAL + 'Home')) {

        CarregaTotais();
        CarregaInfoCards();

        $('#menu-dashboard').attr('style', 'color:#FFF !important;');

    }
});

function CarregaTotais() {
    Loading.start();

    let ObjTotais = '{';
    let now = new Date().getDay() + '/' + new Date().getMonth() + '/' + new Date().getFullYear();
    //ObjTotais += '"datainicio":"' + now + '",';
    //ObjTotais += '"datafim":"' + now + '"';
    ObjTotais += '}';
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Home/GetTotais',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                Loading.done();
                $.each(data, function (index, value) {

                    if (value.TIPO === "LABORATORIO") {

                        $('#qntIdeiasLaboratorio').html(value.VALOR);
                        if (value.VALOR == 1) {
                            $('#qntIdeiasLaboratorio_texto').html('ideia no laboratório');
                        } else {
                            $('#qntIdeiasLaboratorio_texto').html('ideias no laboratório');
                        }
                    }

                    if (value.TIPO === "PROJETO") {

                        $('#qntIdeiasCadastras').html(value.VALOR);
                        if (value.VALOR == 1) {
                            $('#qntIdeiasCadastras_texto').html('ideia concluída');
                        } else {
                            $('#qntIdeiasCadastras_texto').html('ideias concluídas');
                        }
                    }

                    if (value.TIPO === "FEEDBACKS") {

                        $('#qntFeedbacksCadastras').html(value.VALOR);
                        if (value.VALOR == 1) {
                            $('#qntFeedbacksCadastras_texto').html('feedback recebido');
                        } else {
                            $('#qntFeedbacksCadastras_texto').html('feedbacks recebidos');
                        }
                    }

                });
            },
            error: function () {
                Loading.done();
                Messages('error');
            }
        });
}

function CarregaInfoCards() {
    let ObjTotais = '{';
    ObjTotais += '"AVALIACAOENUM":"FEEDBACK"';
    ObjTotais += '}';    
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'Home/BuscarInfoCardsIdeiasAvaliadas',
            data: ObjTotais,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {

            },
            success: function (data) {
                let index = 0;
                while (index < data.length)
                {
                    let quantidadeFeedbacks = $('#id_quantidade_feedbacks_' + data[index].ID).html();

                    if ($('#graph_bar_home_' + data[index].ID).length) {
                        Morris.Bar({
                            element: 'graph_bar_home_' + data[index].ID,
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
                Messages('error');
            }
        });
}

function calculaMediaPontuacao(pontuacao, quantidadeFeedbacks, qtdQuestoesAvaliacao) {
    qtdQuestoesAvaliacao = qtdQuestoesAvaliacao / quantidadeFeedbacks;

    return (pontuacao / (quantidadeFeedbacks * qtdQuestoesAvaliacao)).toFixed(2);
}