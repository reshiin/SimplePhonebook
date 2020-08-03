var profile = function () {
    var onLoad = function (contextRoot) {
        getGroups(contextRoot);

        $('#tblProfiles thead tr th:first').html(
        $('<input/>', {
            type: 'checkbox',
            click: function () {
                var checkboxes = $(this).closest('table').find('tbody tr td input[type="checkbox"]');
                checkboxes.prop('checked', $(this).is(':checked'));
            }
        })
    );
    }

    var changeEvents = function () {
        $('body').on('change', '#fileUpload', function () {
            var input = this;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#imgPreview').prop('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
            }
        });
    };

    var clickEvents = function (contentRoot, contextRoot) {

        $('body').on('click', '#btnAddContact', function () {
            if (!checkForUncommittedChanges()) {
                $('#tblContactNumbers tbody').append('<tr>' +
                    '<td class="d-none">0</td>' +
                    '<td class="d-none">' + $(".btn-save").attr("id") + '</td>' +
                    '<td align="center">' + getContactTypeHtml() + '</td>' +
                    '<td align="center">' + getContactNumberHtml() + '</td>' +
                    '<td align="center"><i class="fa fa-edit" style="cursor: pointer;display: none;"></i>&nbsp;&nbsp;&nbsp;<i class="fa fa-check" style="cursor: pointer;"></i>&nbsp;&nbsp;&nbsp;<i class="fa fa-trash-o" style="cursor: pointer;"></i></td>' +
                '</tr>');
            }
        });

        $('body').on('click', '#btnAddNew', function () {
            clearModalFields(contentRoot);
            $('#modalAddNew').modal('show');
        });

        $('body').on('click', '#btnDelete', function () {
            var ids = "";
            $(".checkbox-item:checked").each(function () {
                ids = $(this).attr("id") + "," + ids
            });
            if (ids != "") {
                if (confirm("Are you sure you want to delete the selected contacts?")) {
                    deleteProfile(ids, contextRoot);
                }
            }
            else toastr.warning("Please select contacts.");
        });

        $('body').on('click', '.link-fullname', function () {
            var id = $(this).attr("id");
            $.ajax({
                type: "POST",
                url: contextRoot + 'GetProfile',
                data: JSON.stringify({ Id: id }),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (!response.status) {
                        toastr.warning(response.message);
                    }
                    else {
                        mapProfile(response.profile, contentRoot);
                        $('#modalAddNew').modal('show');
                    }
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            });
        });

        $('body').on('click', '.fa-check', function () {
            var row = $(this).closest('tr');
            var colType = row.find("td:eq(2)");
            var colNumber = row.find("td:eq(3)");
            var colActions = row.find("td:eq(4)");

            if (colType.find("select").val() == "0")
                toastr.error("Please select contact type.")
            else if (colNumber.find("input").val() == "")
                toastr.error("Please input contact number")
            else {
                var isExist = false;
                $.each($('#tblContactNumbers tbody tr').not(row), function () {
                    var thisColType = $(this).find('td:eq(2)');
                    if (thisColType.html() == colType.find("select").val()) {
                        toastr.error("Contact type already exist.");
                        isExist = true;
                    }
                });

                if (!isExist) {
                    colType.html(colType.find("select").val())
                    colNumber.html(colNumber.find("input").val())
                    colActions.find(".fa-edit").attr("style", "cursor: pointer");
                }
            }
        });

        $('body').on('click', '.fa-edit', function () {
            var row = $(this).closest('tr');
            var colType = row.find("td:eq(2)");
            var colNumber = row.find("td:eq(3)");
            var colActions = row.find("td:eq(4)");

            if (!checkForUncommittedChanges(row)) {
                var contactType = colType.html();
                var contactNumber = colNumber.html();
                colType.html(getContactTypeHtml());
                colType.find("select").val(contactType);
                colNumber.html(getContactNumberHtml());
                colNumber.find('input').val(contactNumber);
                colActions.find(".fa-edit").attr("style", "cursor: pointer;display:none");
            }
        });

        $('body').on('click', '.fa-trash-o', function () {
            $(this).closest('tr').remove();
        });

        $('body').on('click', '.btn-save', function () {
            var profileId = $(this).attr("id");
            if ($("#txtFullName").val().trim() == "")
                toastr.error("Contact Name should not be empty");
            else if ($("#txtEmail").val().trim() == "")
                toastr.error("Email Address should not be empty");
            else if ($("#txtHomeAddress").val().trim() == "")
                toastr.error("Home Address should not be empty");
            else if ($('#tblContactNumbers tbody tr').length == 0)
                toastr.error("Please add at least one contact number");
            else {
                if (!checkForUncommittedChanges()) {
                    var contactNumbers = [];
                    var groupMember = [];
                    $.each($('#tblContactNumbers tbody tr'), function () {
                        var colContactId = $(this).find("td:eq(0)").html();
                        var colProfileId = $(this).find("td:eq(1)").html();
                        var colType = $(this).find("td:eq(2)").html();
                        var colNumber = $(this).find("td:eq(3)").html();
                        contactNumbers.push({ ContactId: colContactId, ProfileId: colProfileId, Type: colType, Number: colNumber });
                    });

                    if ($('#ddlGroups').selectpicker('val').length > 0) {
                        var groupMemberArray = $('#ddlGroups').selectpicker('val').toString().split(',');
                        $.each(groupMemberArray, function (index, groupid) {
                            groupMember.push({ ProfileId: profileId, GroupId: groupid });
                        });
                    }

                    var profile = {
                        Id: profileId,
                        FullName: $("#txtFullName").val().trim(),
                        Email: $("#txtEmail").val().trim(),
                        HomeAddress: $("#txtHomeAddress").val().trim(),
                        ContactNumbers: contactNumbers,
                        GroupMembers: groupMember
                    }

                    var image = $("#fileUpload").get(0).files[0];
                    var formData = new FormData();
                    formData.append("image", image);
                    formData.append("profile", JSON.stringify(profile));
                    addProfile(formData, contextRoot);
                }
            }
        });
    };

    return {
        init: function (contextRoot, contentRoot) {
            onLoad(contextRoot);
            clickEvents(contentRoot, contextRoot);
            changeEvents();
        }
    }
}();

function addProfile(formData, contextRoot) {
    $.ajax({
        type: "POST",
        url: contextRoot + 'AddProfile',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (!response.status) {
                toastr.warning(response.message);
            }
            else {
                $('#modalAddNew').modal('hide');
                $('#btnSearch').click();
                alert('Successfully saved');
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function mapProfile(profile, contentRoot) {
    $("#txtFullName").val(profile.FullName);
    $("#txtEmail").val(profile.Email);
    $("#txtHomeAddress").val(profile.HomeAddress);

    if (profile.ProfilePicture == null)
        $('#imgPreview').prop('src', contentRoot + 'Content/Images/default-profile-picture.jpg');
    else
        $("#imgPreview").attr("src", "data:image/jpg;base64," + arrayBufferToBase64(profile.ProfilePicture));

    var groups = "";
    $.each(profile.GroupMembers, function (i, item) {
        groups = groups + item.GroupId + ',';
    });
    $('#ddlGroups').val(groups.slice(0, -1).split(',')).selectpicker("refresh");

    $('#tblContactNumbers tbody').html('');
    $.each(profile.ContactNumbers, function (i, item) {
        $('#tblContactNumbers tbody').append('<tr>' +
                    '<td class="d-none">' + item.ContactId + '</td>' +
                    '<td class="d-none">' + item.ProfileId + '</td>' +
                    '<td align="center">' + item.Type + '</td>' +
                    '<td align="center">' + item.Number + '</td>' +
                    '<td align="center"><i class="fa fa-edit" style="cursor: pointer;"></i>&nbsp;&nbsp;&nbsp;<i class="fa fa-check" style="cursor: pointer;"></i>&nbsp;&nbsp;&nbsp;<i class="fa fa-trash-o" style="cursor: pointer;"></i></td>' +
                '</tr>');
    });

    $(".modal-title").html("Update Contact");
    $(".btn-save").attr("id", profile.Id)
}

function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function getGroups(contextRoot) {
    $.ajax({
        type: "POST",
        url: contextRoot + 'GetGroups',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (!response.status) {
                toastr.warning(response.message);
            }
            else {
                if (response.groups.length > 0) {
                    $.each(response.groups, function () {
                        var area = this;
                        $("#ddlGroups").append('<option value="' + area.GroupId + '">' + area.GroupName + '</option>').selectpicker('refresh');
                    });
                    $('button.dropdown-toggle').attr('style', 'border:1px solid #ced4da !important');
                }
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function deleteProfile(ids, contextRoot) {

    var data = {
        ids: ids.substring(0, ids.length - 1)
    }
    $.ajax({
        type: "POST",
        url: contextRoot + 'DeleteProfile',
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (!response.status) {
                toastr.warning(response.message);
            }
            else {
                alert("Successfully deleted");
                $('#btnSearch').click();
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
};

function clearModalFields(contentRoot) {
    $('#imgPreview').prop('src', contentRoot + 'Content/Images/default-profile-picture.jpg');
    $('.modal-body').find('input:text').val('');
    $(".selectpicker").val('').selectpicker("refresh");
    $('#tblContactNumbers tbody').html('');
    $("#fileUpload").val('');
    $(".btn-save").attr("id", "0");
    $(".modal-title").html("Add New Contact");
}

function getContactTypeHtml() {
    return '<select class="contact-type form-control form-control-sm">' +
                '<option value="Mobile">Mobile</option>' +
                '<option value="Home">Home</option>' +
                '<option value="Office">Office</option>' +
           '</select>'
}

function getContactNumberHtml() {
    return '<input type="text" maxlength="12" class="contact-number form-control form-control-sm" />';
}

function checkForUncommittedChanges(row) {
    var isExist = false;
    $.each($('#tblContactNumbers tbody tr').not(row), function () {
        if ($(this).find('select').length != 0) {
            toastr.error("Kindly commit existing changes first.");
            isExist = true;
        }
    });

    return isExist;
}

function checkForUncommittedChanges() {
    var isExist = false;
    $.each($('#tblContactNumbers tbody tr'), function () {
        if ($(this).find('select').length != 0) {
            toastr.error("Kindly commit existing changes first.");
            isExist = true;
        }
    });

    return isExist;
}