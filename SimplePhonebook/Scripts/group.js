var group = function () {
    var onLoad = function (contextRoot) {
        $('#tblGroups thead tr th:first').html(
        $('<input/>', {
            type: 'checkbox',
            click: function () {
                var checkboxes = $(this).closest('table').find('tbody tr td input[type="checkbox"]');
                checkboxes.prop('checked', $(this).is(':checked'));
            }
        })
    );
    }

    var clickEvents = function (contextRoot) {
        $('body').on('click', '#btnAddNew', function () {
            $('#txtGroupName').val('');
            $(".btn-save").attr("id", "0");
            showModal(contextRoot, []);
        });

        $('body').on('click', '.btn-controls', function () {
            var control = $(this);
            if (control.val() == ">>") {
                $('#listMembers').append($("#listNonMembers option").clone());
                $('#listNonMembers').html('');
            }
            else if (control.val() == "<<") {
                $('#listNonMembers').append($("#listMembers option").clone());
                $('#listMembers').html('');
            }
            else if (control.val() == ">") {
                var options = $('#listNonMembers option:selected').sort().clone();
                $('#listMembers').append(options);
                $('#listNonMembers option:selected').remove();
            }
            else {
                var options = $('#listMembers option:selected').sort().clone();
                $('#listNonMembers').append(options);
                $('#listMembers option:selected').remove();
            }
        });

        $('body').on('click', '.link-groupname', function () {
            var id = $(this).attr('id');
            var groupName = $(this).html();

            $.ajax({
                type: "POST",
                url: contextRoot + 'GetGroupMembers',
                data: JSON.stringify({ groupId: id }),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (!response.status) {
                        toastr.warning(response.message);
                    }
                    else {
                        $('#txtGroupName').val(groupName);
                        $('.btn-save').attr('id', id);
                        showModal(contextRoot, response.members);
                    }
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            });
        });

        $('body').on('click', '.btn-save', function () {
            var groupId = $(this).attr("id");
            if ($("#txtGroupName").val().trim() == "")
                toastr.error("Group name should not be empty");
            else {
                var groupMembers = [];
                $("#listMembers option").each(function () {
                    groupMembers.push({ ProfileId: this.value, GroupId: groupId })
                });

                var group = {
                    GroupId: groupId,
                    GroupName: $("#txtGroupName").val().trim(),
                    GroupMembers: groupMembers
                }

                saveGroup(group, contextRoot);
            }
        });

        $('body').on('click', '#btnDelete', function () {
            var ids = "";
            $(".checkbox-item:checked").each(function () {
                ids = $(this).attr("id") + "," + ids
            });
            if (ids != "") {
                if (confirm("Are you sure you want to delete the selected groups?")) {
                    deleteGroups(ids, contextRoot);
                }
            }
            else toastr.warning("Please select groups.");
        });
    };


    return {
        init: function (contextRoot) {
            onLoad();
            clickEvents(contextRoot);
        }
    }
}();

function saveGroup(group, contextRoot) {
    $.ajax({
        type: "POST",
        url: contextRoot + 'AddGroup',
        data: JSON.stringify(group),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (!response.status) {
                toastr.warning(response.message);
            }
            else {
                $('#modalAddNew').modal('hide');
                alert("Successfully saved");
                $('#btnSearch').click();
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function deleteGroups(ids, contextRoot) {

    var data = {
        ids: ids.substring(0, ids.length - 1)
    }
    $.ajax({
        type: "POST",
        url: contextRoot + 'DeleteGroup',
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

function showModal(contextRoot, members) {

    $.ajax({
        type: "POST",
        url: contextRoot + 'GetProfiles',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (!response.status) {
                toastr.warning(response.message);
            }
            else {
                $('#listNonMembers').html('');
                $('#listMembers').html('');
                $.each(response.profiles, function(x, item){
                    $('#listNonMembers').append('<option value="' + item.Id + '">' + item.FullName + '</option>');
                });

                if (members.length > 0) {
                    $.each(members, function (x, item) {
                        var member = $('#listNonMembers option[value="' + item.ProfileId + '"]');
                        $('#listMembers').append(member.clone());
                        member.remove();
                    });
                }

                $('#modalAddNew').modal('show');
            }
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
};