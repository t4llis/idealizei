$(document).ready(function () {

    let urlAtual = window.location.href;
    if ( (urlAtual.includes($URL_PRINCIPAL + 'User/Profile/') || urlAtual.includes($URL_PRINCIPAL + 'User/Profile')) && !urlAtual.includes($URL_PRINCIPAL + 'User/ProfileAdm')){

        CarregarIdeiasCocriadas($('#id_user').html());
        BuscarIdeiasEnvolvidas($('#id_user').html());

        CarregarIdealUser($('#id_user').html());

    } else if (urlAtual.includes($URL_PRINCIPAL + 'User/ProfileAdm')) {

        CarregarIdeiasCocriadas('');
        BuscarIdeiasEnvolvidas('');

        CarregarDadosPerfilUser($('#id_adm').html());
        CarregarIdealUser($('#id_adm').html());

        CarregarLinksDeConvite();

        let data_de = "";
        let data_ate = "";
        let usuario_id = "";

        
        $('#tagsAdm').selectize({
            plugins: ['remove_button'],
            delimiter: ',',
            persist: false,
            create: function (input) {
                return {
                    value: input,
                    text: input
                }
            }
        });

        var IntegrantesEquipes = $('#integrantesEquipe').selectize({
            plugins: ['remove_button'],
            delimiter: ',',
            persist: false,
            create: function (input) {
                return {
                    value: input,
                    text: input
                }
            }
        });

        $('#img_loading_pergunta_page3').attr("style", "display:none;");
        $('#img_loading_pergunta_page4').attr("style", "display:none;");

        CarregarEquipes();

        $('.selectize-input').attr("style", "display:flex; cursor:pointer; flex-wrap: wrap;");

        /* INICIO SALVAR TAGS */
        $('#btnSalvarTags').on('click', function () {
            var listaTags = $('#tagsAdm').children();
            let ObjTags = '';

            if (listaTags.length > 0) {
                
                for (var i = 0; i < listaTags.length; i++) {
                    if (listaTags[i].getAttribute('selected') === 'selected') {

                        ObjTags += '{';
                        ObjTags += '"ID":"' + listaTags[i].value + '",';
                        ObjTags += '"ID_ADM":"' + $('#id_adm').html() + '",';
                        ObjTags += '"DESCRICAO":"' + listaTags[i].text + '"';

                        if (i + 1 >= listaTags.length) {
                            ObjTags += '}';
                        } else {
                            ObjTags += '},';
                        }

                    }
                }
            } else {
                ObjTags = '{}';
            }

            ObjTags = '[' + ObjTags + ']';
            $.ajax(
                {
                    type: 'POST',
                    url: $URL_PRINCIPAL + 'User/SalvarTags',
                    data: ObjTags,
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {
                        $('#img_loading_pergunta_page3').attr("style", "display:block;");
                    },
                    success: function (data) {

                        Messages('success', 'SUCESSO!', 'Alteração salva com sucesso.');
                        $('#img_loading_pergunta_page3').attr("style", "display:none;");

                    },
                    error: function () {
                        Messages('error');
                    }
                }
            );
            
        });
        /* FIM SALVAR TAGS */


        /* INICIO SALVAR EQUIPES */
        $('#btnSalvarEquipes').on('click', function () {
            let nomeEquipe = $('#nomeEquipe').val();

            if (nomeEquipe == "") {

                Messages('warning', 'ATENÇÃO!', 'Informe um nome pra a equipe!');

            } else {

                let listaEquipes = $('#integrantesEquipe').children();
                let equipeSelecionada = $('#id_equipe_selecionada').html();

                if (equipeSelecionada == "") {
                    if (listaEquipes.length > 0) {
                        let ObjUsuarios = '';
                        let ObjEquipe = '';
                        let nomeEquipe = $('#nomeEquipe').val();

                        for (var i = 0; i < listaEquipes.length; i++) {
                            if (listaEquipes[i].getAttribute('selected') === 'selected') {

                                ObjUsuarios += '{';
                                ObjUsuarios += '"ID":"' + 0 + '",';
                                ObjUsuarios += '"ID_USER":"' + listaEquipes[i].value + '",';
                                ObjUsuarios += '"ID_EQUIPE":"' + '0' + '"';

                                if (i + 1 >= listaEquipes.length) {
                                    ObjUsuarios += '}';
                                } else {
                                    ObjUsuarios += '},';
                                }

                            }
                        }

                        ObjUsuarios = '"USUARIOS": [' + ObjUsuarios + ']';

                        ObjEquipe += '{';
                        ObjEquipe += '"ID":"' + 0 + '",';
                        ObjEquipe += '"ID_ADM":"' + $('#id_adm').html() + '",';
                        ObjEquipe += '"NOME":"' + nomeEquipe + '",';
                        ObjEquipe += ObjUsuarios + '}';


                        $.ajax(
                            {
                                type: 'POST',
                                url: $URL_PRINCIPAL + 'User/SalvarEquipe',
                                data: ObjEquipe,
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                datatype: 'json',
                                beforeSend: function (xhr) {
                                    $('#img_loading_pergunta_page4').attr("style", "display:block;");
                                },
                                success: function (data) {

                                    CarregarEquipes();

                                    let $select = $('#integrantesEquipe').selectize();
                                    $select[0].selectize.clear();
                                    $('#nomeEquipe').val("");

                                    $('#btnSalvarEquipes').html('Criar Equipe');
                                    Messages('success', 'SUCESSO!', 'Equie salva com sucesso.');
                                    $('#img_loading_pergunta_page4').attr("style", "display:none;");

                                },
                                error: function () {
                                    Messages('error');
                                }
                            }
                        );

                    } else {
                        Messages('warning', 'ATENÇÃO!', 'Selecione ao menos um usuário para a equipe !');
                    }
                } else
                {

                    if (listaEquipes.length > 0) {
                        let ObjUsuarios = '';
                        let ObjEquipe = '';
                        let nomeEquipe = $('#nomeEquipe').val();

                        for (var i = 0; i < listaEquipes.length; i++) {
                            if (listaEquipes[i].getAttribute('selected') === 'selected') {

                                ObjUsuarios += '{';
                                ObjUsuarios += '"ID":"' + 0 + '",';
                                ObjUsuarios += '"ID_USER":"' + listaEquipes[i].value + '",';
                                ObjUsuarios += '"ID_EQUIPE":"' + '0' + '"';

                                if (i + 1 >= listaEquipes.length) {
                                    ObjUsuarios += '}';
                                } else {
                                    ObjUsuarios += '},';
                                }

                            }
                        }

                        ObjUsuarios = '"USUARIOS": [' + ObjUsuarios + ']';

                        ObjEquipe += '{';
                        ObjEquipe += '"ID":"' + equipeSelecionada + '",';
                        ObjEquipe += '"ID_ADM":"' + $('#id_adm').html() + '",';
                        ObjEquipe += '"NOME":"' + nomeEquipe + '",';
                        ObjEquipe += ObjUsuarios + '}';


                        $.ajax(
                            {
                                type: 'POST',
                                url: $URL_PRINCIPAL + 'User/SalvarEquipe',
                                data: ObjEquipe,
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                datatype: 'json',
                                beforeSend: function (xhr) {
                                    $('#img_loading_pergunta_page4').attr("style", "display:block;");
                                },
                                success: function (data) {

                                    CarregarEquipes();

                                    let $select = $('#integrantesEquipe').selectize();
                                    $select[0].selectize.clear();
                                    $('#nomeEquipe').val("");

                                    $('#btnSalvarEquipes').html('Criar Equipe');
                                    Messages('success', 'SUCESSO!', 'Equie salva com sucesso.');
                                    $('#img_loading_pergunta_page4').attr("style", "display:none;");

                                },
                                error: function () {
                                    Messages('error');
                                }
                            }
                        );

                    } else {
                        Messages('warning', 'ATENÇÃO!', 'Selecione ao menos um usuário para a equipe !');
                    }

                }
            }
        });
        /* FIM SALVAR EQUIPES */


        $(document).on("click", ".itens-table-equipes-excluir", function () {
            let idEquipe = $(this).attr("data-equipe");

            $.ajax(
                {
                    type: 'GET',
                    url: $URL_PRINCIPAL + 'User/ExcluirEquipe?idEquipe='+idEquipe,
                    //data: { idEquipe: idEquipe},
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    datatype: 'json',
                    beforeSend: function (xhr) {
                    },
                    success: function (data) {
                        CarregarEquipes();
                        Messages('success', 'SUCESSO!', 'Registro excluido com sucesso!.');
                    },
                    error: function () {
                        Messages('error');
                    }
                }
            );

        });

        $(document).on("click", ".itens-table-equipes-edit", function () {

            $('#btnSalvarEquipes').html('Salvar');

            let idEquipe = $(this).attr("data-equipe");
            let nomeEquipe = $(this).attr("data-nomeEquipe");
            $('#id_equipe_selecionada').html(idEquipe);
            $('#nomeEquipe').val(nomeEquipe);

            let idUsers = $(this).attr("data-idUser");
            var res = idUsers.split(",");
            for (var i = 0; i < res.length; i++) {
                res[i] = parseInt(res[i]);
            }

            let control = IntegrantesEquipes[0].selectize;
            control.setValue(res);

        });

        $('#btnCancelarEdit').on('click', function () {

            let control = IntegrantesEquipes[0].selectize;
            control.clear();

            $('#id_equipe_selecionada').html("");
            $('#nomeEquipe').val("");

            $('#btnSalvarEquipes').html('Criar Equipe');

        });


        $('#pesquisarUser').on('change', function () {

            Loading.start();

            if ($(this).val() != -1) {
                $('#btn_pesquisarUser').html('Limpar');

                $('#id_user_selec').val($(this).val());
                
                CarregarIdeiasCocriadas($(this).val());
                BuscarIdeiasEnvolvidas($(this).val());

                $('#file_adm').attr("style", "display:none;");
                $('#file_adm_label').attr("style", "display:none;");

                CarregarDadosPerfilUser($(this).val());
                CarregarIdealUser($(this).val());

            } else {

                CarregarIdeiasCocriadas('');
                BuscarIdeiasEnvolvidas('');
                $('#file_adm').attr("style", "display:block;");
                $('#file_adm_label').attr("style", "display:block;");

                CarregarDadosPerfilUser($('#id_adm').html());
                CarregarIdealUser($('#id_adm').html());
            }

            Loading.done();

        });

        $('#btn_pesquisarUser').on('click', function () {

            Loading.start();

            if ($('#pesquisarUser').val() != -1) {
                $('#pesquisarUser').val("-1");
                $(this).html('Go!');

                CarregarIdeiasCocriadas('');
                BuscarIdeiasEnvolvidas('');

                CarregarDadosPerfilUser($('#id_adm').html());
                CarregarIdealUser($('#id_adm').html());

                $('#file_adm').attr("style", "display:block;");
                $('#file_adm_label').attr("style", "display:block;");

            }

            Loading.done();

        });

        $('#btnGerarLinkConvite').on('click', function () {

            GerarLinkConvite();

        });
    }
});



function CarregarIdeiasCocriadas(idUser) {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/BuscarTextosCoCriacao?idUser=' + idUser,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $("#respostas_cocriacao").html('');
                $('#img_loading_pergunta_page1').attr("style", "display:block; margin-top:-5px;");
            },
            success: function (data) {
                
                if (data.length > 0) {
                    let id_ideia_old = 0;

                    for (var i = 0; i < data.length; i++) {

                        if (id_ideia_old != data[i].ID_IDEIA) {

                            id_ideia_old = data[i].ID_IDEIA;
                            let li = document.createElement("li");
                            let div = document.createElement("div");
                            div.classList.add("message_wrapper");
                            div.setAttribute("ID", "ID_IDEIA_"+id_ideia_old);

                            let h4 = document.createElement("h4");
                            h4.classList.add("heading");
                            h4.textContent = data[i].NOME_IDEIA;

                            let bloco = document.createElement("blockquote");
                            bloco.classList.add("message");
                            bloco.textContent = data[i].RESPOSTA;

                            let br = document.createElement("br");

                            div.appendChild(h4);
                            div.appendChild(bloco);
                            div.appendChild(br);
                            li.appendChild(div);

                            document.getElementById("respostas_cocriacao").appendChild(li);


                        } else {

                            let bloco = document.createElement("blockquote");
                            bloco.classList.add("message");
                            bloco.textContent = data[i].RESPOSTA;
                            let br = document.createElement("br");

                            document.getElementById("ID_IDEIA_" + id_ideia_old).appendChild(bloco);
                            document.getElementById("ID_IDEIA_" + id_ideia_old).appendChild(br);

                        }
                       
                    }
                }
                $('#img_loading_pergunta_page1').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        }
    );
}

function BuscarIdeiasEnvolvidas(idUser) {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/BuscarIdeiasEnvolvidas?idUser=' + idUser,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $("#IdeiasEnvolvidas").html('');
                $('#img_loading_pergunta_page2').attr("style", "display:block; margin-top:-5px;");
            },
            success: function (data) {

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {


                        let tr = document.createElement("tr");
                        let td1 = document.createElement("td");
                        td1.textContent = (i + 1);

                        let td2 = document.createElement("td");
                        td2.textContent = data[i].NOME_IDEIA;

                        let td3 = document.createElement("td");
                        td3.textContent = data[i].LIDER;

                        tr.appendChild(td1);
                        tr.appendChild(td2);
                        tr.appendChild(td3);

                        document.getElementById("IdeiasEnvolvidas").appendChild(tr);

                    }
                }
                $('#img_loading_pergunta_page2').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        }
    );
}

function CarregarEquipes()
{
    $.ajax(
        {
            type: 'POST',
            url: $URL_PRINCIPAL + 'User/GetAll',
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_pergunta_page4').attr("style", "display:block; margin-top:-5px;");
            },
            success: function (data) {

                if (data.length > 0) {
                    document.getElementById("EquipesCriadas").innerHTML = "";
                    for (var i = 0; i < data.length; i++) {


                        let tr = document.createElement("tr");
                        tr.setAttribute("colspan", "1");

                        let td1 = document.createElement("td");
                        td1.textContent = (i + 1);

                        let td2 = document.createElement("td");
                        td2.textContent = data[i].NOME;

                        let td3 = document.createElement("td");
                        td3.setAttribute("id", "itens-table-equipes-participantes-" + data[i].ID);
                        td3.textContent = data[i].PARTICIPANTES;

                        let td4 = document.createElement("td");
                        td4.classList.add('td-cabecalho-table-equipes');

                        let edit = document.createElement("a");
                        edit.textContent = 'Editar';
                        edit.classList.add("itens-table-equipes");
                        edit.classList.add("itens-table-equipes-edit");
                        edit.setAttribute("id", "itens-table-equipes-edit-" + data[i].ID);
                        edit.setAttribute("data-equipe", data[i].ID);
                        edit.setAttribute("data-idUser", data[i].PARTICIPANTES_ID);
                        edit.setAttribute("data-nomeEquipe", data[i].NOME);

                        let excluir = document.createElement("a");
                        excluir.textContent = 'Excluir';
                        excluir.classList.add("itens-table-equipes");
                        excluir.classList.add("itens-table-equipes-excluir");
                        excluir.setAttribute("id", "itens-table-equipes-excluir-" + data[i].ID);
                        excluir.setAttribute("data-equipe", data[i].ID);

                        td4.appendChild(edit);
                        td4.appendChild(excluir);

                        tr.appendChild(td1);
                        tr.appendChild(td2);
                        tr.appendChild(td3);
                        tr.appendChild(td4);

                        document.getElementById("EquipesCriadas").appendChild(tr);

                    }
                }
                $('#img_loading_pergunta_page4').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        }
    );

}

function CarregarDadosPerfilUser(idUser) {

    let roleAtual = "";
    let NivelAtual = "";
    let urlimagem = "";
    let nomeAtual = "";
    let emailAtual = "";

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/GetPerfilAcessoUser?idUser=' + idUser,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {

                roleAtual = 'ROLE_'+data[0];
                NivelAtual = data[1];
                urlimagem = data[2];
                nomeAtual = data[3];
                emailAtual = data[4];

                if (urlimagem === "") {
                    urlimagem = "/images/user icon.jpeg";
                }

                $('#img_user_1').attr('src', urlimagem);
                $('#img_user_2').attr('src', urlimagem);

                $('#nome_user_label').html(nomeAtual);
                $('#nome_user').val(nomeAtual);

                $('#email_user_label').html(emailAtual);

                // buscar os niveis de acesso
                $.ajax(
                    {
                        type: 'GET',
                        url: $URL_PRINCIPAL + 'User/GetNivelAcesso',
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        datatype: 'json',
                        beforeSend: function (xhr) {
                        },
                        success: function (data) {

                            if (data.length > 0) {

                                document.getElementById("NivelAcesso").innerHTML = '';
                                for (var i = 0; i < data.length; i++) {
                                    let opt = document.createElement("option");
                                    if (data[i] === 'BASICO') {

                                        opt.value = i + 1;
                                        opt.text = 'Básico';
                                        if (data[i] == NivelAtual) {
                                            opt.selected = true;
                                        }

                                    } else if (data[i] === 'INTERMEDIARIO') {

                                        opt.value = i + 1;
                                        opt.text = 'Intermediario';
                                        if (data[i] == NivelAtual) {
                                            opt.selected = true;
                                        }

                                    } else if (data[i] === 'AVANCADO') {

                                        opt.value = i + 1;
                                        opt.text = 'Avançado';
                                        if (data[i] == NivelAtual) {
                                            opt.selected = true;
                                        }

                                    }
                                    document.getElementById("NivelAcesso").appendChild(opt);
                                }
                            }

                        },
                        error: function () {
                            Messages('error');
                        }
                    }
                );

                // buscar os tipos de perfil de usuario
                $.ajax(
                    {
                        type: 'GET',
                        url: $URL_PRINCIPAL + 'User/GetTiposPerfil',
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        datatype: 'json',
                        beforeSend: function (xhr) {
                        },
                        success: function (data) {

                            if (data.length > 0) {

                                document.getElementById("PerfilAcesso").innerHTML = '';
                                for (var i = 0; i < data.length; i++) {

                                    let opt = document.createElement("option");
                                    if (data[i] === 'ROLE_USER') {

                                        opt.value = 'ROLE_USER';
                                        opt.text = 'Normal';
                                        if (data[i] == roleAtual) {
                                            opt.selected = true;
                                        }

                                    } else if (data[i] === 'ROLE_ADMIN') {

                                        opt.value = 'ROLE_ADMIN';
                                        opt.text = 'Admin';
                                        if (data[i] == roleAtual) {
                                            opt.selected = true;
                                        }

                                    }
                                    document.getElementById("PerfilAcesso").appendChild(opt);
                                }
                            }

                        },
                        error: function () {
                            Messages('error');
                        }
                    }
                );

            },
            error: function () {
                Messages('error');
            }
        }
    );

}

function CarregarGridPerfilAdm(idUser, data_de, data_ate) {
    /*
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/GetDadosGraficoPerfilAdmin?idUser=' + idUser + '&data_de=' + data_de + '&data_ate=' + data_ate,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {


                        let tr = document.createElement("tr");
                        let td1 = document.createElement("td");
                        td1.textContent = (i + 1);

                        let td2 = document.createElement("td");
                        td2.textContent = data[i].NOME_IDEIA;

                        let td3 = document.createElement("td");
                        td3.textContent = data[i].LIDER;

                        tr.appendChild(td1);
                        tr.appendChild(td2);
                        tr.appendChild(td3);

                        document.getElementById("IdeiasEnvolvidas").appendChild(tr);

                    }
                }
                $('#img_loading_pergunta_page2').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        });*/
}

function CarregarIdealUser(idUser) {
    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/GetIdeal?idUser=' + idUser,
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
            },
            success: function (data) {

                if (data.length > 0) {

                    $('#graph_bar_user_container').attr('style', 'display:block; margin-top:-5px;');
                    document.getElementById('graph_bar_user').innerHTML = '';
                    Morris.Bar({
                        element: 'graph_bar_user',
                        data: [
                            {
                                device: data[0].LETRA,
                                geekbench: data[0].MEDIA_LETRA.toFixed(2),
                                legenda: data[0].LEGENDA_GRAFICO
                            },
                            {
                                device: data[1].LETRA,
                                geekbench: data[1].MEDIA_LETRA.toFixed(2),
                                legenda: data[1].LEGENDA_GRAFICO
                            },
                            {
                                device: data[2].LETRA,
                                geekbench: data[2].MEDIA_LETRA.toFixed(2),
                                legenda: data[2].LEGENDA_GRAFICO
                            },
                            {
                                device: data[3].LETRA,
                                geekbench: data[3].MEDIA_LETRA.toFixed(2),
                                legenda: data[3].LEGENDA_GRAFICO
                            },
                            {
                                device: data[4].LETRA,
                                geekbench: data[4].MEDIA_LETRA.toFixed(2),
                                legenda: data[4].LEGENDA_GRAFICO
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




                    /*
                    for (var i = 0; i < data.length; i++) {
                        let li = document.createElement("li");
                        let p = document.createElement("p");
                        p.textContent = data[i].LETRA;

                        let div1 = document.createElement("div");
                        div1.classList.add("progress");
                        div1.classList.add("progress_sm");

                        let div2 = document.createElement("div");
                        div2.classList.add("progress-bar");
                        div2.classList.add("bg-green");
                        div2.setAttribute("role","progressbar");
                        div2.setAttribute("data-transitiongoal", data[i].MEDIA_LETRA.toFixed(2) );
                        div2.setAttribute("style", "width: " + (data[i].MEDIA_LETRA.toFixed(2) * 10)+"%; aria-valuenow=100");

                        div1.appendChild(div2);
                        li.appendChild(p);
                        li.appendChild(div1);

                        document.getElementById("items_ideal").appendChild(li);

                    }*/

                } else {
                    $('#graph_bar_user_container').attr('style', 'display:none;');
                }
                
            },
            error: function () {
                Messages('error');
            }
        }
    );

}

function GerarLinkConvite() {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/GerarLinkDeConvite',
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_pergunta_page5').attr("style", "display:block; margin-top:-5px;");
            },
            success: function (data) {

                if (data) {
                    Messages('success', 'SUCESSO!', 'Link gerado com sucesso.');
                    CarregarLinksDeConvite();
                } else {
                    Messages('warning', 'ATENÇÃO!', 'Tivemos problemas ao gerar seu link de convite.');
                }
                $('#img_loading_pergunta_page5').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        }
    );

}

function CarregarLinksDeConvite() {

    $.ajax(
        {
            type: 'GET',
            url: $URL_PRINCIPAL + 'User/BuscarLinksDeConvite',
            async: true,
            contentType: "application/json; charset=utf-8",
            datatype: 'json',
            beforeSend: function (xhr) {
                $('#img_loading_pergunta_page5').attr("style", "display:block; margin-top:-5px;");
            },
            success: function (data) {

                if (data.length > 0) {

                    document.getElementById("LinksConvites").innerHTML = '';
                    for (var i = 0; i < data.length; i++) {
                        let tr = document.createElement("tr");
                        let td1 = document.createElement("td");
                        let td2 = document.createElement("td");
                        td1.textContent = $URL_PRINCIPAL + 'User/AssinarContrato?ticket?=' + data[i];
                        tr.appendChild(td1);

                        document.getElementById("LinksConvites").appendChild(tr);
                    }
                }

                $('#img_loading_pergunta_page5').attr("style", "display:none;");

            },
            error: function () {
                Messages('error');
            }
        }
    );

}