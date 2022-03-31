function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});
function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}

$("#ProductOrderBy").on('change',
    function () {
        $('#filter-form').submit();
    });
//function FillPageId(pageId) {
//    $('#PageId').val(pageId);
//    $('#filter-form').submit();
//}
function OnSuccessRejectItem(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان موفقیت', res.message);
        $('#ajax-url-item-' + res.data.id).hide(300);
        $('#reject-modal-' + res.data.id).modal('toggle');
        $('#reject-modal-' + res.data.id).modal().hide();
        $('.close').click();
    }
}

$("[main_category_checkbox]").on('change',
    function (e) {
        var isChecked = $(this).is(':checked');
        var selectedCategoryId = $(this).attr('main_category_checkbox');
        console.log(selectedCategoryId);
        if (isChecked) {
            $('#sub_categories_' + selectedCategoryId).slideDown(300);
        } else {
            $('#sub_categories_' + selectedCategoryId).slideUp(300);
            $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
        }
    });
$('#add_color_button').on('click',
    function (e) {
        e.preventDefault();
        var colorName = $('#product_color_name_input').val();
        var colorPrice = $('#product_color_price_input').val();
        var colorCode = $('#product_color_code_input').val();
        if (colorName != '' && colorPrice != '' && colorCode != '') {
            var currentColorsCount = $('#list_of_product_colors tr');
            var index = currentColorsCount.length;
            var isExistsSelectedColor = $(
                '[coler-name-hidden-index][value="' + colorName + '"]'
            );
            if (isExistsSelectedColor.length === 0) {
                var colorNameNode =
                    `<input type="hidden" value="${colorName}"  name="ProductColors[${index
                    }].ColorName" coler-name-hidden-index="${colorName}-${colorPrice}" >`;
                var colorPriceNode =
                    `<input type="hidden" value="${colorPrice}"  name="ProductColors[${index
                    }].Price"  coler-price-hidden-index="${colorName}-${colorPrice}" >`;
                var colorCodeNode = `<input type="hidden" value="${colorCode}"  name="ProductColors[${index}].ColorCode" color-code-hidden-input="${colorName}-${colorPrice}" >`;
                $('#create_product_form').append(colorNameNode);
                $('#create_product_form').append(colorPriceNode);
                $('#create_product_form').append(colorCodeNode);
                var colorTableNode =
                    `<tr coler-item-item="${colorName}-${colorPrice}"> <td> ${colorName} </td>  <td> ${colorPrice
                    } </td> <td> <div style="border-radius: 50%;width: 40px; height: 40px; background-color:${colorCode}"></div> </td> <td> <a class="btn btn-danger" onclick="removeProductColer('${colorName}-${colorPrice
                    }')" >حذف</a> </td>  </tr>`;
                $('#list_of_product_colors').append(colorTableNode);

                $('#product_color_name_input').val('');
                $('#product_color_price_input').val('');
                $('#product_color_code_input').val('');
            } else {
                ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
                $('#product_color_name_input').val('').focus();
            }
        } else {
            ShowMessage("اخطار", "اطفا رنگ را درست وارد کنید", "warning")
        }
    });




//$('#add_feature_button').on('click',
//    function (e) {
//        e.preventDefault();
//        var feature = $('#product_feature_input').val();
//        var featureValue = $('#product_feature_value_input').val();
//        if (feature !== '' && featureValue !== '') {
//            var currentFeaturesCount = $('#list_of_product_features tr');
//            var index = currentFeaturesCount.length;

//            var isExistsSelectedColor = $('[feature-hidden-input][value="' + feature + '"]');
//            if (isExistsSelectedColor.length === 0) {
//                var featureNode = `<input type="hidden" value="${feature}"  name="ProductFeatures[${index}].ColorName" feature-hidden-input="${feature}-${featureValue}">`;
//                var featureValueNode = `<input type="hidden" value="${featureValue}"  name="ProductFeatures[${index}].Price" feature-hidden-input="${feature}-${featureValue}" >`;
//                $('#create_product_form').append(featureNode);
//                $('#create_product_form').append(featureValueNode);

//                var featureTableNode = `<tr feature-table-item="${feature}-${featureValue}"> <td> ${feature} </td>  <td> ${featureValue} </td>  <td> <a class="btn btn-danger text-white" onclick="removeProductFeature('${feature}-${featureValue}')">حذف</a> </td>  </tr>`;
//                $('#list_of_product_features').append(featureTableNode);


//                $('#product_feature_input').val('');
//                $('#product_feature_value_input').val('');
//            } else {
//                ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
//                $('#product_color_name_input').val('').focus();
//            }
//        } else {
//            ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
//        }
//    }
//);


//function removeProductFeature(index) {
//    console.log(index);
//    $('[feature-hidden-input="' + index + '"]').remove();
//    $('[feature-value-hidden-input="' + index + '"]').remove();
//    $('[feature-table-item="' + index + '"]').remove();
//}




$('#add_feature_button').on('click',
    function (e) {
        e.preventDefault();
        var feature = $('#product_feature_input').val();
        var featureValue = $('#product_feature_value_input').val();
        if (feature !== '' && featureValue !== '') {
            var currentFeaturesCount = $('#list_of_product_features tr');
            var index = currentFeaturesCount.length;

            var isExistsSelectedColor = $('[feature-hidden-input][value="' + feature + '"]');
            if (isExistsSelectedColor.length === 0) {
                var featureNode = `<input type="hidden" value="${feature}"  name="ProductFeatures[${index}].Feature" feature-hidden-input="${feature}-${featureValue}">`;
                var featureValueNode = `<input type="hidden" value="${featureValue}"  name="ProductFeatures[${index}].FeatureValue" feature-value-hidden-input="${feature}-${featureValue}" >`;
                $('#create_product_form').append(featureNode);
                $('#create_product_form').append(featureValueNode);

                var featureTableNode = `<tr feature-table-item="${feature}-${featureValue}"> <td> ${feature} </td>  <td> ${featureValue} </td>  <td> <a class="btn btn-danger text-white" onclick="removeProductFeature('${feature}-${featureValue}')">حذف</a> </td>  </tr>`;
                $('#list_of_product_features').append(featureTableNode);


                $('#product_feature_input').val('');
                $('#product_feature_value_input').val('');
            } else {
                ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
                $('#product_feature_input').val('').focus();
            }
        } else {
            ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
        }
    }
);


function removeProductFeature(index) {
    console.log(index);
    $('[feature-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();
    $('[feature-table-item="' + index + '"]').remove();
    reOrderProductFeatureHiddenInputs();
}


function reOrderProductFeatureHiddenInputs() {
    var hiddenFeatures = $('[feature-hidden-input]');
    $.each(hiddenFeatures, function (index, value) {
        var hiddenFeature = $(value);
        var featureId = $(value).attr('feature-hidden-input');
        var featureValue = $('[feature-value-hidden-input="' + featureId + '"]');
        $(hiddenFeature).attr('name', 'ProductFeatures[' + index + '].Feature');
        $(featureValue).attr('name', 'ProductFeatures[' + index + '].FeatureValue');
        console.log(hiddenFeature);
        console.log(featureValue);
    });
}



function removeProductColer(index2) {
    $('[coler-item-item="' + index2 + '"]').remove();
    $('[coler-name-hidden-index="' + index2 + '"]').remove();
    $('[coler-price-hidden-index="' + index2 + '"]').remove();
    $('[color-code-hidden-input="' + index2 + '"]').remove();
    reOrderProductColorHiddenInputs();
}
function reOrderProductColorHiddenInputs() {
    var hiddenColors = $('[coler-name-hidden-index]');
    $.each(hiddenColors, function (index, value) {
        var hiddenColor = $(value);
        var colorId = $(value).attr('coler-name-hidden-index');
        var hiddenCode = $('[color-code-hidden-input="' + colorId + '"]');
        var hiddenPrice = $('[coler-price-hidden-index="' + colorId + '"]');
        $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName');
        $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        $(hiddenCode).attr('name', 'ProductColors[' + index + '].ColorCode');
    });


    function changeProductPriceBasedOnColor(priceOfColor, colorName) {
        var basePrice = parseInt($('#ProductBasePrice').val(), 0);
        $('.current_price').html((basePrice + priceOfColor) + ' تومان' + ' ( ' + colorName + ' )');
    }
}