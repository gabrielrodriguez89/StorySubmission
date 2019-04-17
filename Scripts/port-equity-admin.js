var previousSelectedType = -1;
var $QuestionBtn = $("button#question-btn");
var questionStatusBtn = ".question-status-btn";
var statusChangeBtn = ".stories-status-btn";
var StoriesPublishOn = ".stories-publish-on";
var StoriesDelete = "#stories-delete-confirm-btn";
var StoriesDeleteModal = "#Stories-delete-modal";
var DeleteModal = $(StoriesDeleteModal);

function changeStatus(buttonUi, StoriesId, publicStatus, isQuestion) {
    IsActive = publicStatus.toLowerCase() === "true";

    var text = IsActive ? "Not Live" : "Live";

    Messenger().run({
        action: $.ajax,
        successMessage: "Successfully Toggled to " + text,
        errorMessage: "Error Toggling to " + text,
        progressMessage: "Toggling to " + text
    }, {
            method: "PATCH",
            url: "/EquityStories/ChangeStoryPublicStatus/",
            data: { Id: StoriesId, IsActive: !IsActive, Question: isQuestion },
            success: function () {
                if (!isQuestion) {
                    if (IsActive) {
                        buttonUi.data("stories-public", "False");
                        buttonUi.removeClass("btn-success").addClass("btn-danger").text("Not Live");
                    } else {
                        buttonUi.data("stories-public", "True");
                        buttonUi.removeClass("btn-danger").addClass("btn-success").text("Live");
                    }
                }
                else {
                    if (IsActive) {
                        buttonUi.data("stories-public", "False");
                        buttonUi.removeClass("btn-success").addClass("btn-danger").text("Not Live");
                    } else {
                        buttonUi.data("stories-public", "True");
                        buttonUi.removeClass("btn-danger").addClass("btn-success").text("Live");
                    }
                }

            }
        });
}
function Delete(Id, url, callback) {
    Messenger().run({
        action: $.ajax,
        successMessage: "Successfully Deleted",
        errorMessage: "Error Deleting ",
        progressMessage: "Deleting..."
    }, {
        method: "DELETE",
        url: url,
        data: { Id: Id },
        success: function () {
            DeleteModal.modal('hide');
            if (callback) {
                callback();
            }
        }
    });
}

function initializeEditables() {

    var currentYear = new Date().getFullYear();
    var minYear = currentYear - 5;
    var maxYear = currentYear + 5;

    var comboDateProperties = {
        combodate: {
            minYear: minYear,
            maxYear: maxYear,
            minuteStep: 1,
            firststory: 'name'
        }
    };
}
function dataTableCallback(htmlString) {
    $("#StoriesTable tbody").empty().html(htmlString);
   
    initializeEditables();
}

$(document).on("show.bs.modal", StoriesDeleteModal, function (event) {
    var button = $(event.relatedTarget);
    var StoriesId = button.data("stories-id");

    // Assign stories Id to the button
    $(this).find("#stories-delete-confirm-btn").data("stories-id", StoriesId);
});
$(document).on("click", questionStatusBtn, function () {
    var StoriesId = $(this).data("stories-id");
    var status = $(this).data("stories-public");

    changeStatus($(this), StoriesId, status, true);
});
$(document).on("click", statusChangeBtn, function () {
    var StoriesId = $(this).data("stories-id");
    var status = $(this).data("stories-public");

    changeStatus($(this), StoriesId, status, false);
});

$(document).on("click", StoriesDelete, function () {
    var el = this;
    var StoriesId = $(el).data("stories-id");
    var url = "";
    if (!questions) {
        url = "/EquityStories/DeleteStory/";
    }
    else {
        url = "/EquityStories/DeleteQuestion/";
    }
    Delete(StoriesId, url, function () {
        $("tr[data-stories-id=" + StoriesId + "]").remove();
    });
});