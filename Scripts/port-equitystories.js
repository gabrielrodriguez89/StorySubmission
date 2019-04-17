/*
 *
 * Equity Success Stories Widget
 * ------------------------------------
 * Version 1.1.0
 * Development Date: October 22, 2018
 * Created By: Gabriel Rodriguez
 * Dependencies: jquery.ui
 * 
 */


function selectEmployee() {
    var selected = $("#ModalEmployee").select2().find(":selected");
    var employee = selected.data("name");
    var phone = selected.data("phone");
    var email = selected.data("email");
    var userID = selected[0].value;

    $("#SelectedQuestion_CreatedBy").val(employee);
    $("#ContactName").val(employee);
    $("#ContactEmail").val(email);
    $("#ContactPhone").val(phone);
    $("#UserName").val(userID);
}

$("#BtnSelectEmployee").on("click", function () {
    $("#SelectEmployeeModal").modal("hide");
    selectEmployee();
});

function validate() {
    var count = 0;

    if ($("#Description").val() === undefined || $("#Description").val().length === 0) {
        $(".description").text("*Please enter a short description");
        count = count + 1;
    }
    if ($("#Title").val() === undefined || $("#Title").val().length === 0) {
        $(".title").text("*Title is Required");
        count = count + 1;
    }
    return count > 0 ? false : true;
}

$(function () {
    $("#Description").bind('keyup', function () {
        if ($("#Description").val().length > 150) {
            $(".description").text("Warning, you have reached over 150 characters. Please reduce the text to continue.");
        }
        else {
            $(".description").text("");
        }
    });
    $("#Title").blur(function () {
        if ($("#Title").val() === undefined || $("#Title").val().length === 0) {
            $(".title").text("*Title is Required");
        }
        else {
            $(".title").text("");
        }
    });
    $("#Description").blur(function () {
        if ($("#Description").val() === undefined || $("#Description").val().length === 0) {
            $(".description").text("*Please enter a short description");
        }
        else {
            $(".description").text("");
        }
    });

    $("textarea.create").focus(function () {
        $(this).animate({ height: "9em" }, 400);
    });
    $("textarea.create").blur(function () {
        $(this).animate({ height: "2.8em" }, 400);
    });
    (function ($, undefined) {
        $.widget("Port.EquityStories", {
            options: {
                debug: true,
                equityStoriesUrl: "/EquityStories/LazyLoadStories",
                addStoryUrl: "/EquityStories/CreateStory",
                saveNewStoryUrl: "/EquityStories/CreateStory",
                imagePathGenerator: "",
                containerClass: "",
                displayInitialStories: true,
                maxTake: 0,
                length: 0,
                divCount: 0,
                first: true,
                lazy: true,
                widgetId: null,
                maxHeight: null,
                refresh: null,
                destroy: null,
                getStory: "",
                container: ""
            },

            refresh: function () {
                this.options.divCount = this.options.divCount + 1;
               
            },

            _create: function () {
                var that = this;
                this.errorCount = 0;
                

                this.transportMethods = { Get: "GET", POST: "POST", PUT: "PUT", DELETE: "DELETE" };
                if (this.options.divCount > 0) {
                    this.lazyCounter = { start: this.lazyCounter.start, take: this.options.maxTake };
                }
                else {
                    this.lazyCounter = { start: 0, take: this.options.maxTake };
                }
                
                that._createContainer();
               
                that._getStories(true);

                this._on(this.element, {
                    'click.ess-lazyload-bar': function (event) {
                        that._lazyLoad(event);
                    }
                });

                
            },

            _createContainer: function () {
                if (this.options.lazy) {
                    var $lazyDiv = $("<div class='ess-lazyload-bar'></div>");
                    $lazyDiv.append($("<h4>Loading Questions</h4>"));
                    
                }
                if (this.options.widgetId !== null) {
                    this.options.container = $(".ess-container");
                }
                else {
                    this.options.container = $("<div class='ess-container" + this.options.divCount.toString()  + "'></div>");
                }
                this.element.append(this.options.container);

            },

            _getStoriesHtml: function (stories) {
                var that = this;
                var story = stories;
                var date = new Date(parseInt(story.PublishDate.substr(6)));
                var createdOn = this._parseDate(date, "mmmm dd, yyyy");
                var imageUrl = this.options.imagePathGenerator(story.UserName);
                var options = that.options;
                
                var info = "";
                var description = story.Description !== null ? story.Description : story.Content.substr(0, 150) + "...";

                if (this.options.widgetId !== null) {

                    info =
                        "<table class='ess-story'><tbody><tr>"
                        + "<td><div class='media-body'><div class='media-heading'>"
                        + "<p><a href='/EquityStories/View/" + story.Id + "'>" + story.Title + "</a> - <strong>" + story.ContactName + "</strong><span class='ess-title'></span></p></div>"
                        + "<div class='ess-content'>" + description + "</div>"
                        + "</td>"
                        + "</tr></tbody></table>";
                }
                else {
                    var button = "";
                    
                    info =
                        "<table class='ess-story'><tbody><tr>"
                        + "<td><div class='media-body'><div class='col-sm-12'>"
                        + "<h3><a href='/EquityStories/View/" + story.Id + "'>" + story.Title + "</a></h3>"
                        + "<small>" + story.ContactName + "</a></small><br/>"
                        + "<div class='ess-content'>" + description + "</div>"
                        + "<div class='message-footer' style='text-align:left;'><a href='/EquityStories/View/" + story.Id + "'>Read More</a></div>"
                        + "</div></div>"
                        + "</td>"
                        + "</tr></tbody></table>";
                            
                }

                return $("<li data-storyId='" + story.Id + "'></li>").html(info);
            },
            _addButton: function () {
                var that = this;
                $(".btn-div").append("<input type='button' class='btn btn-primary next-story-btn' name='next' value='Next' />");
                $(".btn-div").on("click", ".next-story-btn", function (event) {
                    that.refresh();
                    that._create();
                });
                   
            },
            _getStories: function (updateLatestStoryId) {
                var that = this;
                var options = that.options;
                var url = options.equityStoriesUrl + "?start=" + that.lazyCounter.start + "&take=" + that.lazyCounter.take;

                if (!that.options.displayInitialStories) return;

                that._transport(that.transportMethods.Get, url, null)
                    .done(function (stories) {
                        count = stories["length"] - 1;
                        if (stories.length > 0) {
                            
                            that.options.first = false;
                            var updateStart = parseInt(that.lazyCounter.start) + parseInt(options.maxTake);
                            that.lazyCounter.start = updateStart;

                            that._renderStories(stories, that);

                            if (updateLatestStoryId) {
                                that.lazyCounter.start = that.options.length;
                            }
                            if (that.options.length >= that.lazyCounter.take) {
                                that._addButton();

                            }
                        }
                        else {
                            if (that.options.widgetId === null) {
                                that._noStories();
                            }
                            else {
                                $("#" + that.options.widgetId).show();
                                that._endOfStories(true);
                            }

                        }
                        
                        
                        $("#" + that.options.widgetId + " .footer").show();
                        that._resetErrors();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        that._transportError(jqXHR, textStatus, errorThrown, that);
                        that._endOfStories();
                        $("#" + that.options.widgetId).find(".loading").removeClass("active");
                        $("#" + that.options.widgetId).find(".ess-container")
                            .html("<p style='padding:0 15px;'>There was an error getting the question. Please refresh your page and try again.");
                    })
                    .complete(function () {
                        if (that.options.widgetId !== null) {
                            $("#" + that.options.widgetId).find(".loading").removeClass("active");
                        }
                        else {
                            $(".equity-widget").find("h4").remove();
                        }
                    });
            },

            _renderStories: function (stories) {
                var that = this;
                length = stories.length;
                that.options.length = length;

                if (length > 0) {
                    var $ul = $("<ul class='ess-stories'></ul>");
                    for (var i = 0; i < length; i++) {
                        var $li = this._getStoriesHtml(stories[i]);

                        $ul.append($li);
                    }
                }
                $(".more-stories").find("h4").remove();
                this.options.container.append($ul);
                that._endOfStories(false);
                $("#" + that.options.widgetId).find(".loading").removeClass("active");

            },

            _endOfStories: function (end) {
                if (this.options.lazy) {
                    if (end) {
                        $(".ess-lazyload-bar").removeClass("error").addClass("end");
                        $(".ess-lazyload-bar").find("h4").text("No More Stories");
                    }
                    else {
                        $(".ess-lazyload-bar").removeClass("error");
                        $(".ess-lazyload-bar").find("h4").text("More Questions");
                    }
                }
            },

            _noStories: function () {
                if (this.options.first) {
                    $(".more-stories").find("h4").remove();
                    $(".more-stories").append($("<h1>Success Stories Coming Soon</h1>"));
                    $(".more-stories").append($("<p>Be the first to share your story, just click the button above!</p>"));
                }
                else {
                    var that = this;
                    $(".more-stories").append("<h4>No More Stories</h4>");
                    $(".btn-div input").remove();
                    
                }
            },
            _resetErrors: function () {
                if (this.errorCount > 0) {
                    this.errorCount = 0;
                }
            },

            _displayError: function () {
                if (!$(".ess-lazyload-bar").is(".error")) {
                    // Print out that there is an error
                    $(".ess-lazyload-bar").removeClass("no-more").addClass("error");
                    $(".ess-lazyload-bar").find("h4").text("Error Encountered!");
                }
            },

            _lazyLoad: function (evt) {
                var that = this;

                if ($(evt.currentTarget).is(".no-more, .error")) {
                    return;
                }

                that._showLoading();

                // Don't update to the latest story Id.
                that._getStories(false);
            },

            _showLoading: function () {
                if (this.options.lazy) {
                    $(".ess-lazyload-bar").find("h4").text("Loading Questions");
                }
            },

            _getUserPic: function (userId) {
                return window.location.origin + "/Resources/Photo/" + userId + "?height=147&width=105";
            },

            _transport: function (method, transportUrl, postData) {
                var that = this;
                var ajaxSettings = { method: method };

                if (method === that.transportMethods.POST) {
                    ajaxSettings.data = JSON.stringify(postData);
                    ajaxSettings.contentType = "application/json; charset=utf-8"
                }

                var ajaxReady = $.ajax(transportUrl, ajaxSettings);

                return ajaxReady;
            },

            _transportError: function (jqXHR, textStatus, errorThrown) {

                // Print out the transport error information if enabled
                if (this.options.debug) {
                    console.log(jqXHR);
                    console.log(textStatus);
                    console.log(errorThrown);
                }

                // Increase the count of error to delay the stream get
                this.errorCount++;

                this._displayError();
            },

            _parseDate: function (date, mask, utc) {

                /*
                * Date Format 1.2.3
                * (c) 2007-2009 Steven Levithan <stevenlevithan.com>
                * MIT license
                *
                * Includes enhancements by Scott Trenda <scott.trenda.net>
                * and Kris Kowal <cixar.com/~kris.kowal/>
                *
                * Accepts a date, a mask, or a date and a mask.
                * Returns a formatted version of the given date.
                * The date defaults to the current date/time.
                * The mask defaults to dateFormat.masks.default.
                */

                var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
                    timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
                    timezoneClip = /[^-+\dA-Z]/g,
                    pad = function (val, len) {
                        val = String(val);
                        len = len || 2;
                        while (val.length < len) val = "0" + val;
                        return val;
                    };


                // Some common format strings
                var masks = {
                    "default": "ddd mmm dd yyyy HH:MM:ss",
                    shortDate: "m/d/yy",
                    mediumDate: "mmm d, yyyy",
                    longDate: "mmmm d, yyyy",
                    fullDate: "dddd, mmmm d, yyyy",
                    shortTime: "h:MM TT",
                    mediumTime: "h:MM:ss TT",
                    longTime: "h:MM:ss TT Z",
                    isoDate: "yyyy-mm-dd",
                    isoTime: "HH:MM:ss",
                    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
                    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
                };

                // Internationalization strings
                var i18n = {
                    dayNames: [
                        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
                        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
                    ],
                    monthNames: [
                        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
                        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
                    ]
                };

                // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
                if (arguments.length === 1 && Object.prototype.toString.call(date) === "[object String]" && !/\d/.test(date)) {
                    mask = date;
                    date = undefined;
                }

                // Passing date through Date applies Date.parse, if necessary
                date = date ? new Date(date) : new Date;
                if (isNaN(date)) throw SyntaxError("invalid date");

                mask = String(masks[mask] || mask || masks["default"]);

                // Allow setting the utc argument via the mask
                if (mask.slice(0, 4) === "UTC:") {
                    mask = mask.slice(4);
                    utc = true;
                }

                var _ = utc ? "getUTC" : "get",
                    d = date[_ + "Date"](),
                    D = date[_ + "Day"](),
                    m = date[_ + "Month"](),
                    y = date[_ + "FullYear"](),
                    H = date[_ + "Hours"](),
                    M = date[_ + "Minutes"](),
                    s = date[_ + "Seconds"](),
                    L = date[_ + "Milliseconds"](),
                    o = utc ? 0 : date.getTimezoneOffset(),
                    flags = {
                        d: d,
                        dd: pad(d),
                        ddd: i18n.dayNames[D],
                        dddd: i18n.dayNames[D + 7],
                        m: m + 1,
                        mm: pad(m + 1),
                        mmm: i18n.monthNames[m],
                        mmmm: i18n.monthNames[m + 12],
                        yy: String(y).slice(2),
                        yyyy: y,
                        h: H % 12 || 12,
                        hh: pad(H % 12 || 12),
                        H: H,
                        HH: pad(H),
                        M: M,
                        MM: pad(M),
                        s: s,
                        ss: pad(s),
                        l: pad(L, 3),
                        L: pad(L > 99 ? Math.round(L / 10) : L),
                        t: H < 12 ? "a" : "p",
                        tt: H < 12 ? "am" : "pm",
                        T: H < 12 ? "A" : "P",
                        TT: H < 12 ? "AM" : "PM",
                        Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                        o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                        S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 !== 10) * d % 10]
                    };

                return mask.replace(token, function ($0) {
                    return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
                });

                // For convenience...
                //Date.prototype.format = function (mask, utc) {
                //    return dateFormat(this, mask, utc);
                //};

            }
        });
    }(jQuery));
});