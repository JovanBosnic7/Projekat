$(function () {
    
    var dataGlobal;
    getDataForMorris("", 0);

    $('#card-donut').hide();
    $('#datum-area-evidentirane').show();

    $('#datum-area-izbrisane, #datum-area-evidentirane-poreklo').hide();
    $('#datum-bar-izbrisane, #datum-bar-evidentirane-poreklo').hide();
    $('#datum-bar-stacked-izbrisane, #datum-bar-stacked-evidentirane-poreklo').hide();
   

    $('#izvEvidentiranihLine').click(function () {

        $('#lbl_naslov_line, #lbl_naslov_bar, #lbl_naslov_bar_stacked').text($('#izvEvidentiranihLine').text());
        $('#morris-line-izbrisane-chart').hide();
        $('#morris-line-evidentirane-poreklo-chart').hide();
        $('#morris-line-evidentirane-chart').show();

        $('#card-donut').hide();
        $('#datum-area-evidentirane, #datum-bar-evidentirane, #datum-bar-stacked-evidentirane').show();
        $('#datum-area-izbrisane, #datum-bar-izbrisane, #datum-bar-stacked-izbrisane ').hide();
        $('#datum-area-evidentirane-poreklo, #datum-bar-evidentirane-poreklo, #datum-bar-stacked-evidentirane-poreklo').hide();

        
        getDataForMorris("", 0, false);
       

    });

    $('#izvEvidentiranihBar').click(function () {

        $('#lbl_naslov_bar').text($('#izvEvidentiranihBar').text());
        $('#morris-bar-izbrisane-chart').hide();
        $('#morris-bar-evidentirane-poreklo-chart').hide();
        $('#morris-bar-evidentirane-chart').show();

        $('#datum-bar-evidentirane').show();
        $('#datum-area-izbrisane, #datum-bar-izbrisane, #datum-bar-stacked-izbrisane ').hide();
        $('#datum-area-evidentirane-poreklo, #datum-bar-evidentirane-poreklo, #datum-bar-stacked-evidentirane-poreklo').hide();

        drawBarChartEvidentirane("", false, dataGlobal);
        //$('#datum-area-evidentirane').data('dateRangePicker').clear();

    });

    $('#izvEvidentiranihBarStacked').click(function () {

        $('#lbl_naslov_bar_stacked').text($('#izvEvidentiranihBarStacked').text());
        $('#morris-bar-stacked-izbrisane-chart').hide();
        $('#morris-bar-stacked-evidentirane-poreklo-chart').hide();
        $('#morris-bar-stacked-evidentirane-chart').show();

        $('#datum-bar-stacked-evidentirane').show();
        $('#datum-area-izbrisane, #datum-bar-izbrisane, #datum-bar-stacked-izbrisane ').hide();
        $('#datum-area-evidentirane-poreklo, #datum-bar-evidentirane-poreklo, #datum-bar-stacked-evidentirane-poreklo').hide();

        drawBarStackedChartEvidentirane("", false, dataGlobal);
        $('#datum-area-evidentirane').data('dateRangePicker').clear();

    });

    

    $('#izvProsecnaCenaLine').click(function () {

        $('#card-donut').hide();
        $('#lbl_naslov_donut, #lbl_naslov_area, #lbl_naslov_line, #lbl_naslov_area, #lbl_naslov_bar, #lbl_naslov_bar_stacked').text($('#izvProsecnaCenaLine').text());
        $('#morris-area-izbrisane-chart, #morris-line-izbrisane-chart, #morris-bar-izbrisane-chart, #morris-bar-stacked-izbrisane-chart, #morris-donut-izbrisane-chart  ').hide();
        $('#morris-area-evidentirane-poreklo-chart, #morris-line-evidentirane-poreklo-chart, #morris-bar-evidentirane-poreklo-chart, #morris-bar-stacked-evidentirane-poreklo-chart, #morris-donut-evidentirane-poreklo-chart ').hide();
        $('#morris-area-evidentirane-chart, #morris-line-evidentirane-chart, #morris-bar-evidentirane-chart, #morris-bar-stacked-evidentirane-chart, #morris-donut-evidentirane-chart ').show();


        getDataForMorris("", 0, true);

    });

    $('#izvProsecnaCenaBar').click(function () {

        $('#lbl_naslov_bar').text($('#izvProsecnaCenaBar').text());
        $('#morris-bar-izbrisane-chart').hide();
        $('#morris-bar-evidentirane-poreklo-chart').hide();
        $('#morris-bar-evidentirane-chart').show();

        drawBarChartEvidentirane("", true, dataGlobal);
        $('#datum-area-evidentirane').data('dateRangePicker').clear();

    });

    $('#izvProsecnaCenaBarStacked').click(function () {

        $('#lbl_naslov_bar_stacked').text($('#izvProsecnaCenaBarStacked').text());
        $('#morris-bar-stacked-izbrisane-chart').hide();
        $('#morris-bar-stacked-evidentirane-poreklo-chart').hide();
        $('#morris-bar-stacked-evidentirane-chart').show();

        drawBarStackedChartEvidentirane("", true, dataGlobal);
        $('#datum-area-evidentirane').data('dateRangePicker').clear();

    });

    

    /* KLIK NA DATUM - range nema za dijagram evidentirane i cene */
    $('#datum-area-evidentirane').dateRangePicker(
        {
        autoClose: true,
        singleDate: true,
        showShortcuts: false,
        endDate: moment().endOf('day').format('YYYY-MM-DD') 
        })
        .bind('datepicker-closed', function () {
            var izabranDatumArea = $('#datum-area-evidentirane').val();
            var izabranIzv = $('#lbl_naslov_line').text();
            var lblNaslov = "";

            if (izabranDatumArea !== "") {
                if (izabranIzv.includes('Evidentirani paketi')) {
                    getDataForMorris(izabranDatumArea, 0, false);
                    lblNaslov = 'Evidentirani paketi';
                }
                else if (izabranIzv.includes('Prosecna cena posiljka / paket')) {
                    lblNaslov = 'Prosecna cena posiljka / paket';
                    getDataForMorris(izabranDatumArea, 0, true);
                }
                   
                
            }
            $('#lbl_naslov_line, #lbl_naslov_bar, #lbl_naslov_bar_stacked').text(lblNaslov + " " + izabranDatumArea); 
            $('#datum-area-evidentirane').data('dateRangePicker').clear();
        });

    /* KLIK NA DATUM - range nema za dijagram evidentirane i cene */
    $('#datum-bar-evidentirane').dateRangePicker(
        {
            autoClose: true,
            singleDate: true,
            showShortcuts: false,
            endDate: moment().endOf('day').format('YYYY-MM-DD') 

        })
        .bind('datepicker-closed', function () {
            var izabranDatumBar = $('#datum-bar-evidentirane').val();
            var izabranIzv = $('#lbl_naslov_bar').text();
            var lblNaslov = "";

            if (izabranDatumBar !== "") {
                if (izabranIzv.includes('Evidentirani paketi')) {
                    getDataForMorris(izabranDatumBar, 2, false);
                    lblNaslov = 'Evidentirani paketi';
                }

                else if (izabranIzv.includes('Prosecna cena posiljka / paket')) {
                    getDataForMorris(izabranDatumBar, 2, true);
                    lblNaslov = 'Prosecna cena posiljka / paket';
                }
                    

            }
            $('#lbl_naslov_bar').text(lblNaslov + " " + izabranDatumBar);
            $('#datum-bar-evidentirane').data('dateRangePicker').clear();
        });

    /* KLIK NA DATUM - range nema za dijagram evidentirane i cene */
    $('#datum-bar-stacked-evidentirane').dateRangePicker(
        {
            autoClose: true,
            singleDate: true,
            showShortcuts: false,
            endDate: moment().endOf('day').format('YYYY-MM-DD') 

        })
        .bind('datepicker-closed', function () {
            var izabranDatumBarStacked = $('#datum-bar-stacked-evidentirane').val();
            var izabranIzv = $('#lbl_naslov_bar_stacked').text();
            var lblNaslov = "";

            if (izabranDatumBarStacked !== "") {
                if (izabranIzv.includes('Evidentirani paketi')) {
                    getDataForMorris(izabranDatumBarStacked, 3, false);
                    lblNaslov = 'Evidentirani paketi';
                }
                    
                else if (izabranIzv.includes('Prosecna cena posiljka / paket')) {
                    getDataForMorris(izabranDatumBarStacked, 3, true);
                    lblNaslov = 'Prosecna cena posiljka / paket';
                }
                    

            }
            $('#lbl_naslov_bar_stacked').text(lblNaslov + " " + izabranDatumBarStacked);
            $('#datum-bar-stacked-evidentirane').data('dateRangePicker').clear();
        });

    function IsJsonString(str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
    }

    function getDataForMorris(izabranDatum, tipMorris, isCena) {
        /* tipMorris je tip dijagrama koji se ucitava
           0 - ucitaj sve
           1 - Area
           2 - Line
           3 - Bar
           4 - BarStacked
         */

        $("#loadingDIV").show();

        $.ajax({
            url: "Home/GetEvidentiraneChart",
            type: 'get',
            data: {
                dateForChart: izabranDatum
            },
            success: function (data) {
                console.log("Data: " + data);
                if (!IsJsonString(data)) {
                   
                    alert("Nemate prava pristupa izveštajima o evidentiranim pošiljkama i cenama.");
                    $("#loadingDIV").hide();
                    return;
                } 
                dataGlobal = data;
                if (tipMorris === 0) {
                    //drawAreaChartEvidentirane(izabranDatum, isCena);
                    fillTableEvidentirane(data, izabranDatum);
                    drawLineChartEvidentirane(izabranDatum, isCena, data);
                    drawBarChartEvidentirane(izabranDatum, isCena, data);
                    drawBarStackedChartEvidentirane(izabranDatum, isCena, data);
                } 
                else if (tipMorris === 2)
                    drawBarChartEvidentirane(izabranDatum, isCena, data);
                else if (tipMorris === 3)
                    drawBarStackedChartEvidentirane(izabranDatum, isCena, data);
                else if (tipMorris === 4)
                    drawLineChartEvidentirane(izabranDatum, isCena, data);

                //else if (tipMorris === 1)
                //    drawAreaChartEvidentirane(izabranDatum, isCena);

                $("#loadingDIV").hide();

            },

            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }  
        });

        //return dataLoad;
    }

    /** EVIDENTIRANE **/
    

    function fillTableEvidentirane(data, izabranDatum)
    {
        
       

        var datDanas;
        if (izabranDatum === "") {
            datDanas = new Date();
        } else {
            datDanas = new Date(izabranDatum);
        }

        var dat7Dana = new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr");
        var dat14Dana = new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr");
        var dat21Dan = new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr");

        var evid1Posiljke = 0;
        var evid2Posiljke = 0;
        var evid3Posiljke = 0;
        var evid4Posiljke = 0;
        var evid1Paketa = 0;
        var evid2Paketa = 0;
        var evid3Paketa = 0;
        var evid4Paketa = 0;
        var pakPos1 = 0;
        var pakPos2 = 0;
        var pakPos3 = 0;
        var pakPos4 = 0;
        var dataJson = $.parseJSON(data);

        for (var prop in dataJson) {
           
            evid1Posiljke = parseInt(dataJson[prop].evidentiraneUkupno);
            evid2Posiljke = parseInt(dataJson[prop].evidentiraneUkupno1Nedelja);
            evid3Posiljke = parseInt(dataJson[prop].evidentiraneUkupno2Nedelja);
            evid4Posiljke = parseInt(dataJson[prop].evidentiraneUkupno3Nedelja);

            evid1Paketa = parseInt(dataJson[prop].evidentiranoPaketa);
            evid2Paketa = parseInt(dataJson[prop].evidentiranoPaketa1);
            evid3Paketa = parseInt(dataJson[prop].evidentiranoPaketa2);
            evid4Paketa = parseInt(dataJson[prop].evidentiranoPaketa3);
        }

        pakPos1 = Math.round((evid1Paketa / evid1Posiljke)*100)/100;
        pakPos2 = Math.round((evid2Paketa / evid2Posiljke) * 100) / 100;
        pakPos3 = Math.round((evid3Paketa / evid3Posiljke) * 100) / 100;
        pakPos4 = Math.round((evid4Paketa / evid4Posiljke) * 100) / 100;

        $("#tableEvidentirane tbody").empty();

        var newRowContent = "<tr><td>" + datDanas.toLocaleDateString("sr") + "</td><td>" + evid1Paketa + "</td><td>" + evid1Posiljke + "</td><td>" + pakPos1 + "</td> </tr>";
        var newRowContent1 = "<tr><td>" + dat7Dana + "</td><td>" + evid2Paketa + "</td><td>" + evid2Posiljke + "</td><td>" + pakPos2 + "</td> </tr>";
        var newRowContent2 = "<tr><td>" + dat14Dana + "</td><td>" + evid3Paketa + "</td><td>" + evid3Posiljke + "</td><td>" + pakPos3 + "</td> </tr>";
        var newRowContent3 = "<tr><td>" + dat21Dan + "</td><td>" + evid4Paketa + "</td><td>" + evid4Posiljke + "</td><td>" + pakPos4 + "</td> </tr>";

        $("#tableEvidentirane tbody").append(newRowContent); 
        $("#tableEvidentirane tbody").append(newRowContent1); 
        $("#tableEvidentirane tbody").append(newRowContent2); 
        $("#tableEvidentirane tbody").append(newRowContent3); 

    }


    
    function drawLineChartEvidentirane(izabranDatum, isCena, dataForMorris) {
        $("#morris-line-evidentirane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        var datDanas;
        if (isCena) {
            ykeys.push('cenaPoPosiljci');
            ykeys.push('cenaPoPosiljci1');
            ykeys.push('cenaPoPosiljci2');
            ykeys.push('cenaPoPosiljci3');
        } else {
            //ykeys.push('evidentiraneUkupno');
            //ykeys.push('evidentiraneUkupno1Nedelja');
            //ykeys.push('evidentiraneUkupno2Nedelja');
            //ykeys.push('evidentiraneUkupno3Nedelja');

            /********* Na osnovu ovoga se prikazuju podaci na dijagramu *****/

            ykeys.push('evidentiranoPaketa');
            ykeys.push('evidentiranoPaketa1');
            ykeys.push('evidentiranoPaketa2');
            ykeys.push('evidentiranoPaketa3');
        }

        lineColors.push('#d9534f', 'black', 'gray', 'orange');

        if (izabranDatum === "") {
            datDanas = new Date();
        } else {
            datDanas = new Date(izabranDatum);
        }

        console.log("drawLineChartEvidentirane: " + izabranDatum);

        Morris.Line({
            element: 'morris-line-evidentirane-chart',
            data: $.parseJSON(dataForMorris),
            xkey: ['period'],
            ykeys: ykeys,
            labels: ykeys,
            pointSize: 3,
            hideHover: 'auto',
            hoverCallback: function (index, options, content, row) {

                var trHTML = '<table>';
                if (isCena) {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Cena po paketu: ' + row.cenaPoPaketu + '; Cena po pošiljci: ' + row.cenaPoPosiljci + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Cena po paketu: ' + row.cenaPoPaketu1 + '; Cena po pošiljci: ' + row.cenaPoPosiljci1 + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Cena po paketu: ' + row.cenaPoPaketu2 + '; Cena po pošiljci: ' + row.cenaPoPosiljci2 + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange; "><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange; "><td> Cena po paketu: ' + row.cenaPoPaketu3 + '; Cena po pošiljci: ' + row.cenaPoPosiljci3 + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';

                } else {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Paketa: ' + row.evidentiranoPaketa + '; Pošiljki: ' + row.evidentiraneUkupno + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Paketa: ' + row.evidentiranoPaketa1 + '; Pošiljki: ' + row.evidentiraneUkupno1Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Paketa: ' + row.evidentiranoPaketa2 + '; Pošiljki: ' + row.evidentiraneUkupno2Nedelja + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange;"><td> Paketa: ' + row.evidentiranoPaketa3 + '; Pošiljki: ' + row.evidentiraneUkupno3Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';
                }
                trHTML += ' </tr></table > ';

                return trHTML;
            },
            resize: true,
            parseTime: false,
            fillOpacity: 0.6,
            behaveLikeLine: true,
            pointFillColors: ['#ffffff'],
            pointStrokeColors: ['black'],
            lineColors: lineColors
        });
    }

    function drawBarChartEvidentirane(izabranDatum, isCena, dataForMorris) {
        $("#morris-bar-evidentirane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        var datDanas;
        if (isCena) {
            ykeys.push('cenaPoPosiljci');
            ykeys.push('cenaPoPosiljci1');
            ykeys.push('cenaPoPosiljci2');
            ykeys.push('cenaPoPosiljci3');
        } else {
            //ykeys.push('evidentiraneUkupno');
            //ykeys.push('evidentiraneUkupno1Nedelja');
            //ykeys.push('evidentiraneUkupno2Nedelja');
            //ykeys.push('evidentiraneUkupno3Nedelja');
            ykeys.push('evidentiranoPaketa');
            ykeys.push('evidentiranoPaketa1');
            ykeys.push('evidentiranoPaketa2');
            ykeys.push('evidentiranoPaketa3');
        }

        lineColors.push('#d9534f', 'black', 'gray', 'orange');

        if (izabranDatum === "") {
            datDanas = new Date();
        } else {
            datDanas = new Date(izabranDatum);
        }

        console.log("drawBarChartEvidentirane: " + izabranDatum);

        Morris.Bar({
            element: 'morris-bar-evidentirane-chart',
            data: $.parseJSON(dataForMorris),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            hideHover: 'auto',
            hoverCallback: function (index, options, content, row) {

                var trHTML = '<table>';
                if (isCena) {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Cena po paketu: ' + row.cenaPoPaketu + '; Cena po pošiljci: ' + row.cenaPoPosiljci  + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Cena po paketu: ' + row.cenaPoPaketu1 + '; Cena po pošiljci: ' + row.cenaPoPosiljci1  + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Cena po paketu: ' + row.cenaPoPaketu2 + '; Cena po pošiljci: ' + row.cenaPoPosiljci2  + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange;"><td> Cena po paketu: ' + row.cenaPoPaketu3 + '; Cena po pošiljci: ' + row.cenaPoPosiljci3  + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';

                } else {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Paketa: ' + row.evidentiranoPaketa + '; Pošiljki: ' + row.evidentiraneUkupno  + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Paketa: ' + row.evidentiranoPaketa1 + '; Pošiljki: ' + row.evidentiraneUkupno1Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Paketa: ' + row.evidentiranoPaketa2 + '; Pošiljki: ' + row.evidentiraneUkupno2Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange;"><td> Paketa: ' + row.evidentiranoPaketa3 + '; Pošiljki: ' + row.evidentiraneUkupno3Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';
                }
                return trHTML;
            },
            resize: true,
            barColors: lineColors
        });
    }

    function drawBarStackedChartEvidentirane(izabranDatum, isCena, dataForMorris) {
        $("#morris-bar-stacked-evidentirane-chart").empty();
        var ykeys = [];
        var lineColors = [];
        var datDanas;
        if (isCena) {
            ykeys.push('cenaPoPosiljci');
            ykeys.push('cenaPoPosiljci1');
            ykeys.push('cenaPoPosiljci2');
            ykeys.push('cenaPoPosiljci3');
        } else {
            ykeys.push('evidentiranoPaketa');
            ykeys.push('evidentiranoPaketa1');
            ykeys.push('evidentiranoPaketa2');
            ykeys.push('evidentiranoPaketa3');
            //ykeys.push('evidentiraneUkupno');
            //ykeys.push('evidentiraneUkupno1Nedelja');
            //ykeys.push('evidentiraneUkupno2Nedelja');
            //ykeys.push('evidentiraneUkupno3Nedelja');
        }

        lineColors.push('#d9534f', 'black', 'gray', 'orange');

        if (izabranDatum === "") {
            datDanas = new Date();
        } else {
            datDanas = new Date(izabranDatum);
        }

        console.log("drawBarStackedChartEvidentirane: " + izabranDatum);

        Morris.Bar({
            element: 'morris-bar-stacked-evidentirane-chart',
            data: $.parseJSON(dataForMorris),
            xkey: 'period',
            ykeys: ykeys,
            labels: ykeys,
            stacked: true,
            hideHover: 'auto',
            hoverCallback: function (index, options, content, row) {

                var trHTML = '<table>';
                if (isCena) {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Cena po paketu: ' + row.cenaPoPaketu + '; Cena po pošiljci: ' + row.cenaPoPosiljci  + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Cena po paketu: ' + row.cenaPoPaketu1 + '; Cena po pošiljci: ' + row.cenaPoPosiljci1  + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Cena po paketu: ' + row.cenaPoPaketu2 + '; Cena po pošiljci: ' + row.cenaPoPosiljci2  + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange;"><td> Cena po paketu: ' + row.cenaPoPaketu3 + '; Cena po pošiljci: ' + row.cenaPoPosiljci3  + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';

                } else {
                    trHTML += '<tr style="color:#d9534f;"><td>' + datDanas.toLocaleDateString("sr") + '</td></tr>';
                    trHTML += '<tr style="color:#d9534f;"><td> Paketa: ' + row.evidentiranoPaketa + '; Pošiljki: ' + row.evidentiraneUkupno + '; Pak/Pos: ' + row.odnosPaketPosiljka + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:black;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 7)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:black;"><td> Paketa: ' + row.evidentiranoPaketa1 + '; Pošiljki: ' + row.evidentiraneUkupno1Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka1 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:gray;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 14)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:gray;"><td> Paketa: ' + row.evidentiranoPaketa2 + '; Pošiljki: ' + row.evidentiraneUkupno2Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka2 + '</td></tr>';
                    trHTML += '<tr><td>&nbsp;</td></tr>';
                    trHTML += '<tr style="color:orange;"><td>' + new Date(new Date().setDate(datDanas.getDate() - 21)).toLocaleDateString("sr"); + '</td></tr>';
                    trHTML += '<tr style="color:orange;"><td> Paketa: ' + row.evidentiranoPaketa3 + '; Pošiljki: ' + row.evidentiraneUkupno3Nedelja  + '; Pak/Pos: ' + row.odnosPaketPosiljka3 + '</td>';
                }

                return trHTML;
            },
            resize: true,
            barColors: lineColors
        });
    }
});