$(document).ready(function () {

    var TipoIdeia = "";
    var nomeIdeia = "";
    var nomeLider = "";

    var TextoIdeiaInovadora = "";
    var TextoQueProblemas = "";
    var TextoPratica = "";
    var TextoResultados = "";
    var TextoImpacto = "";
    var linkDoPitIdeia = "";
    var TemCocriador = false;

    $(".ideia_visible_nv2").addClass('itens_invisible');
    $(".ideia_visible_nv3").addClass('itens_invisible');
    $("#linkCompartilharIdeia").text($URL_PRINCIPAL);

    $('.btn-prox_nv1').on("click", function () {
        let nomeIdeia = $('#input_nome_ideia').val();
        if (nomeIdeia.trim() == '') {
            Messages('warning', 'ATENÇÃO!', 'Obrigatório informar o nome da ideia!');
            $('#input_nome_ideia').focus();            
            return;
        }

        TipoIdeia = $(this).attr("data-valor");

        $(".ideia_visible_nv1").addClass('itens_invisible');
        $(".ideia_visible_nv2").removeClass('itens_invisible');

        window.location.href = $URL_PRINCIPAL + 'Ideia/Edit?idIdeia=0&TipoIdeia=1&nomeIdeia=' + nomeIdeia;
    });

    $('.btn-prox_nv2').on("click", function () {

        nomeIdeia = $("#nomeIdeia").val();
        nomeLider = $("#LiderIdeia").val();
        if ($('#radio_sim').is(':checked')) {
            TemCocriador = true;
        }

        $(".ideia_visible_nv2").addClass('itens_invisible');
        $(".ideia_visible_nv3").removeClass("itens_invisible");
    });

    $('.btn-prox_nv3').on("click", function () {

        Loading.start();

        nomeIdeia = $("#nomeIdeia").val();
        nomeIdeia = nomeIdeia.replaceAll('"', "'");

        if (nomeIdeia == '') {
            Messages('warning', 'ATENÇÃO!', 'Obrigatório informar o nome da ideia!');
            $('#nomeIdeia').focus();
            Loading.done();
            return;
        }

        nomeLider = $("#LiderIdeia").val();
        if ($('#radio_sim').is(':checked')) {
            TemCocriador = true;
        }

        TextoIdeiaInovadora = $("#TextoIdeiaInovadora").val();
        TextoIdeiaInovadora = TextoIdeiaInovadora.replaceAll('"', "'");

        TextoQueProblemas = $("#TextoQueProblemas").val();
        TextoQueProblemas = TextoQueProblemas.replaceAll('"', "'");

        TextoPratica = $("#TextoPratica").val();
        TextoPratica = TextoPratica.replaceAll('"', "'");

        TextoResultados = $("#TextoResultados").val();
        TextoResultados = TextoResultados.replaceAll('"', "'");

        TextoImpacto = $("#TextoImpacto").val();
        TextoImpacto = TextoImpacto.replaceAll('"', "'");

        linkDoPitIdeia = $("#linkDoPitIdeia").val();

        let ObjNovaIdeia = '{';

        ObjNovaIdeia += '"NOMEPROJETO":"' + nomeIdeia + '",';
        ObjNovaIdeia += '"TIPOPROJETO":"' + TipoIdeia + '",';
        ObjNovaIdeia += '"TEXTOIDEIAINOVADORA":"' + TextoIdeiaInovadora + '",';
        ObjNovaIdeia += '"TEXTOPROBLEMAS":"' + TextoQueProblemas + '",';
        ObjNovaIdeia += '"TEXTOPRATICA":"' + TextoPratica + '",';
        ObjNovaIdeia += '"TEXTORESULTADOS":"' + TextoResultados + '",';
        ObjNovaIdeia += '"TEXTOIMPACTO":"' + TextoImpacto + '",';

        if (TemCocriador) {
            ObjNovaIdeia += '"STATUS":"1",';
        } else {
            ObjNovaIdeia += '"STATUS":"2",';
        }

        if (linkDoPitIdeia == "") { 
            ObjNovaIdeia += '"LINKPIT": " "';
        } else {
            ObjNovaIdeia += '"LINKPIT":"' + linkDoPitIdeia + '"';
        }
        ObjNovaIdeia += '}';

        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/CadastrarIdeia',
                data: ObjNovaIdeia,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {

                },
                success: function (data) {
                    Loading.done();
                    if (data != 0) {
                        Messages('success', 'SUCESSO!', 'Ideia cadastrada com sucesso');

                        if (TemCocriador) {
                            window.location.href = $URL_PRINCIPAL + 'Ideia/Share';
                        }
                        else {
                            window.location.href = $URL_PRINCIPAL + 'Ideia/Share/?idIdeia=' + data + '&shareEnum=AVALIACAO';
                        }
                    }
                    else {
                        Loading.done();
                        Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao cadastrar sua ideia, tente mais tarde.');
                    }                        
                },
                error: function () {
                    Loading.done();
                    Messages('error');
                }
            });

    });

    $('.btn-Modal-duplicar-ideia').on('click', function () {

        Loading.start();
        let idIdeia = $(this).attr("data-idideia");
        let nomeIdeiaDuplicada = $('#nomeIdeiaDuplicada').val();

        if (nomeIdeiaDuplicada.trim() == '') {
            Loading.done();
            Messages('warning', 'ATENÇÃO!', 'Obrigatório informar o nome da ideia!');
            $('#nomeIdeiaDuplicada').focus();
            return;
        }

        ValidaNomeIdeia(nomeIdeiaDuplicada, idIdeia);
    });

    $('.btn-salvar-novo-pit').on('click', function () {
        Loading.start();

        let linkNovoPit = $('#link-novo-pit').val();
        let idIdeia = $('#id_ideia_selecioanda').html();
        
        let ObjTotais = '{';
        ObjTotais += '"idieia":"' + idIdeia + '",';
        ObjTotais += '"linkPit":"' + linkNovoPit + '"';
        ObjTotais += '}';
        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/EditLinkPit/',
                data: ObjTotais,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {

                },
                success: function (data) {
                    Loading.done();
                    Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');
                    location.reload();
                },
                error: function (error) {
                    Loading.done();
                    Messages('error');
                }
            });

    });

    function ValidaNomeIdeia(nomeIdeiaDuplicada, idIdeia) {        
        let ObjNovaIdeia = '{"nomeIdeia": "' + nomeIdeiaDuplicada + '"}';

        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/ValidaNomeIdeia',
                data: ObjNovaIdeia,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {

                },
                success: function (data) {                                        
                    if (data) {                        
                        DuplicarIdeia(nomeIdeiaDuplicada, idIdeia);
                    }
                    else {         
                        Loading.done();
                        Messages('warning', 'ATENÇÃO!', 'Você já tem uma ideia com este nome!');
                        $('#nomeIdeiaDuplicada').focus();
                    }                        
                },
                error: function () {
                    Loading.done();
                    Messages('error');
                }
            });

    }    

    function DuplicarIdeia(nomeIdeiaDuplicada, idIdeia) {

        let ObjNovaIdeia = '{"idIdeiaOrigem": "' + idIdeia + '", ';
        ObjNovaIdeia += '"nomeIdeia": "' + nomeIdeiaDuplicada + '"}';

        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/DuplicarIdeia',
                data: ObjNovaIdeia,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {

                },
                success: function (data) {
                    Loading.done();
                    if (data != 0) {
                        Messages('success', 'SUCESSO!', 'Ideia duplicada com sucesso');
                        window.location.href = $URL_PRINCIPAL + 'Ideia/Edit/?idIdeia=' + data;
                    }
                    else
                        Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao cadastrar sua ideia, tente mais tarde.');
                },
                error: function () {
                    Loading.done();
                    Messages('error');
                }
            });

    }

    $('.div_editar_ideia').on('click', function () {

        $(".ideia_cadastrada_edit").val('');

        let textoIdeiaCadastrada = $(this).context.children[0].innerText;
        $(".ideia_cadastrada_edit").val(textoIdeiaCadastrada);       

        $("#id_questao_ideias_edit").html('');
        $("#id_ideia_selecionada_edit").html('');

        $("#id_questao_ideias_edit").html($(this).attr('data-id_questao_ideias_edit'));
        $("#id_ideia_selecionada_edit").html($(this).attr('data-id_ideia_selecionada_edit'));
    });

    $('.btn_Modal_editar_ideia').on('click', function () {     

        let textoIdeiaCadastrada = $('.ideia_cadastrada_edit').val();
        textoIdeiaCadastrada = textoIdeiaCadastrada.trim(); 
        textoIdeiaCadastrada = textoIdeiaCadastrada.replaceAll('"', "'");

        if (textoIdeiaCadastrada == '') {            
            Messages('warning', 'ATENÇÃO!', 'Obrigatório informar a ideia!');
            $('.ideia_cadastrada_edit').focus();
            return;
        }

        let idQuestaoIdeia = $('#id_questao_ideias_edit').html();
        let idIdeiaSelecionada = $('#id_ideia_selecionada_edit').html();        

        EditIdeia(idIdeiaSelecionada, idQuestaoIdeia, textoIdeiaCadastrada);
    });

    function EditIdeia(idIdeiaSelecionada, idQuestaoIdeia, textoIdeiaCadastrada) {
        Loading.start();

        let ObjTotais = '{';
        ObjTotais += '"ID":"' + idIdeiaSelecionada + '",';
        ObjTotais += '"ID_QUESTAO_IDEIAS":"' + idQuestaoIdeia + '",';
        ObjTotais += '"TEXTOIDEIA":"' + textoIdeiaCadastrada + '"';
        ObjTotais += '}';
        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Ideia/EditIdeia',
                data: ObjTotais,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                beforeSend: function (xhr) {

                },
                success: function (data) {
                    Loading.done();
                    Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');
                    location.reload();
                },
                error: function (error) {
                    Loading.done();
                    Messages('error');
                }
            });
    }



    let urlAtual = window.location.href;
    if ((urlAtual.toUpperCase() == ($URL_PRINCIPAL + 'ideia/Index/').toUpperCase()) ||
        (urlAtual.toUpperCase().includes(($URL_PRINCIPAL + 'ideia/Index/?aba=').toUpperCase()))) {

        CarregarIdeiasConcluidas();
        CarregarIdeiasEmAndamento();

        $('table').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
            }
        })
        $('#table_ideias_em_andamento').DataTable();
        $('#table_ideias_concluidas').DataTable();
    }


});




function CarregarIdeiasConcluidas() {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarIdeiasConcluidas',
            async: false,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_ideias_concluidas').attr("style", "display:block;");
            },
            success: function (data) {
                
                for (var i = 0; i < data.length; i++) {
                    
                    let tr = document.createElement('tr');

                    let tdimg = document.createElement('td');
                    let img = document.createElement('img');
                    img.setAttribute("title", "ideia");
                    img.setAttribute("alt", "ideia");
                    img.setAttribute("style", "width:40px;");
                    img.setAttribute("src", data[i].URLIMAGEM);
                    tdimg.appendChild(img);
                    tr.appendChild(tdimg);

                    let tdnome = document.createElement('td');
                    tdnome.textContent = data[i].NOME_IDEIA;
                    tr.appendChild(tdnome);

                    let tdMedia = document.createElement('td');
                    tdMedia.setAttribute("class", "text-center");
                    tdMedia.textContent = data[i].MEDIA == 0 ? "" : leadingZeros(data[i].MEDIA);
                    tr.appendChild(tdMedia);


                    let tdlink = document.createElement('td');
                    let alink = document.createElement('a');
                    let link = "/Ideia/Edit?idIdeia=" + data[i].IDHASH;
                    alink.setAttribute("href", link);
                    alink.setAttribute("class", "btn btn-light btn-ideias btn-ideia-selecionada");
                    alink.textContent = "Clique para visualizar";
                    tdlink.appendChild(alink);

                    tr.appendChild(tdlink);

                    $('#conteudo_ideias_concluidas').append(tr);


                }
                $('#img_loading_ideias_concluidas').attr("style", "display:none;");

            },
            error: function (error) {
                Messages('error');
                $('#img_loading_ideias_concluidas').attr("style", "display:none;");
            }
        });
}

function leadingZeros(nr) {   
    return Math.abs(nr).toFixed(2);    
}

function CarregarIdeiasEmAndamento() {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'Ideia/BuscarIdeiasEmAndamento',
            async: false,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_ideias_em_andamento').attr("style", "display:block;");
            },
            success: function (data) {

                document.getElementById('conteudo_ideias_em_andamento').innerHTML = '';
                for (var i = 0; i < data.length; i++) {

                    let tr = document.createElement('tr');
                    
                    let tdimg = document.createElement('td');
                    let img = document.createElement('img');
                    img.setAttribute("title", "ideia");
                    img.setAttribute("alt", "ideia");
                    img.setAttribute("style", "width:40px;");
                    img.setAttribute("src", data[i].URLIMAGEM);
                    tdimg.appendChild(img);
                    tr.appendChild(tdimg);

                    let tddata = document.createElement('td');
                    let dataCriacao = moment(data[i].DATA_CRIACAO).toDate();
                    let dataCriacaoFormatada = moment(dataCriacao).format("DD/MM/YYYY hh:mm:ss");
                    tddata.textContent = dataCriacaoFormatada;
                    tr.appendChild(tddata);

                    let tdnome = document.createElement('td');
                    tdnome.textContent = data[i].NOME_IDEIA;
                    tr.appendChild(tdnome);

                    let tdlink = document.createElement('td');
                    let alink = document.createElement('a');
                    let link = "/Ideia/Edit?idIdeia=" + data[i].IDHASH;
                    alink.setAttribute("href", link);
                    alink.setAttribute("class", "btn btn-light btn-ideias btn-ideia-selecionada");
                    alink.textContent = "Clique para visualizar";
                    tdlink.appendChild(alink);

                    tr.appendChild(tdlink);
                    
                    $('#conteudo_ideias_em_andamento').append(tr);
                    
                }
                $('#img_loading_ideias_em_andamento').attr("style", "display:none;");

            },
            error: function() {
                Messages('error');
                $('#img_loading_ideias_em_andamento').attr("style", "display:none;");
            }
        }
    );
}
