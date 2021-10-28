//-----------------------------------------------------------------------
// <copyright file="kingcounty-ptas-b2c.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

// (function($) {
//   $.fn.kcgov_b2c_override = function() {
//     console.log('starting-plugin');
//     var $this = $(this);

// $('.b2c-wrapper').after($('<div class="working"></div>'));

// $('.verificationInfoText')
//   .parent()
//   .addClass('errorMessageSection');
// /*identify the current form*/
// if (
//   location.href
//     .toLowerCase()
//     .indexOf('B2c_1A_PTAS_DEV_signup_signin_with_magic_link') > 0
// ) {
//   $this.addClass('sign-in');

/*fix error box behavior*/
//   $('.b2c-wrapper .error.itemLevel').each(function() {
//     $(this)
//       .next()
//       .after($(this));
//   });

//   $('input#logonIdentifier, input#password').focus(function() {
//     $(this)
//       .parent()
//       .find('.error')
//       .addClass('hideError');
//   });
//   $('input#logonIdentifier, input#password').blur(function() {
//     $(this)
//       .parent()
//       .find('.error')
//       .removeClass('hideError');
//   });

//   $this.find('.localAccount .intro').hide();
//   $this.find('.localAccount .entry').hide();
//   $this
//     .find('.localAccount')
//     .prepend(
//       $(
//         '<div class="buttons"><button id="show-local">' +
//           $this.find('.localAccount .intro h2').text() +
//           '</button></div>'
//       )
//     );
//   $this.find('.localAccount #show-local').click(function() {
//     $('.b2c-wrapper .social').hide();
//     $('.b2c-wrapper .divider').hide();
//     $('.b2c-wrapper .localAccount #show-local').hide();
//     $('.b2c-wrapper .localAccount .intro').show();
//     $('.b2c-wrapper .localAccount .entry').show();
//   });
// } else if (
//   location.href.toLowerCase().indexOf('/api/selfasserted/confirmed') > 0
// ) {
//console.log('ENTERED api/selfasserted/confirmed');
//$this.find('#confirmed-no-email').removeClass('hidden');

// console.log('reset-password');
// $this.addClass('reset-password');
// /*fix error box behavior*/
// $('.b2c-wrapper .error.itemLevel').each(function() {
//   $(this)
//     .next()
//     .next()
//     .after($(this));
// });
// $('input#reenterPassword')
//   .parent()
//   .parent()
//   .hide();
// $('input#newPassword').change(function() {
//   $('input#reenterPassword').val($('input#newPassword').val());
// });
// $('input#reenterPassword, input#newPassword').focus(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .addClass('hideError');
// });
// $('input#reenterPassword, input#newPassword').blur(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .removeClass('hideError');
// });
// } else if (
//   location.href.toLowerCase().indexOf('b2c_1_ptas_dev_senior_reset') > 0
// ) {
// console.log('recover-password');
// $this.addClass('recover-password');
// /*fix error box behavior*/
// $('.b2c-wrapper .error.itemLevel').each(function() {
//   $(this)
//     .next()
//     .next()
//     .after($(this));
// });
// $('input#email, input#newPassword').focus(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .addClass('hideError');
// });
// $('input#email, input#newPassword').blur(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .removeClass('hideError');
// });
// $('.recover-password .buttons').hide();
// $('.recover-password button#cancel').hide();
// $('.recover-password input')
//   .parent()
//   .hide();
// $('label[for="email"]').addClass('shrink');
// $('input#email')
//   .parent()
//   .show();
// $('input#email')
//   .parent()
//   .find('.buttons')
//   .show();
// $('input#email').focus();
// $('#attributeList ul').before($('.errorMessageSection'));
// $('#email_ver_but_default, #email_ver_but_send').click(function() {
//   $('.validate .error').removeClass('show');
//   $('label[for="email"]').addClass('shrink');
// });
// $('#email_ver_but_send').click(function() {
//   $('input#email')
//     .parent()
//     .find('.helpLink')
//     .hide();
//   $('label[for="email"], input#email').hide();
// });
// $('#email_ver_but_verify').click(function() {
//   $('.attrEntry.validate > div:first-child').css({
//     position: 'absolute',
//     top: '5rem'
//   });
//   $('#email_ver_input').after($('.attrEntry.validate > div:first-child'));
//   $('.working').show();
//   setTimeout(function() {
//     $('.working').hide();
//     if (!$('.verificationErrorText').is(':visible')) {
//       $('input#email')
//         .parent()
//         .find('.helpLink')
//         .show();
//       $('label[for="email"], input#email').show();
//       $('.recover-password .buttons').show();
//       $('button#continue').prop('disabled', false);
//       $('button#email_ver_but_edit').click(function() {
//         $('input#newPassword')
//           .parent()
//           .hide();
//         $('input#givenName')
//           .parent()
//           .hide();
//         $('input#surname')
//           .parent()
//           .hide();
//         $('.recover-password .buttons').hide();
//         $('.recover-password .buttons.verify').show();
//         $('input#email').prop('disabled', false);
//         $('input#email').focus();
//       });
//     }
//   }, 2000);
// });
// } else if (location.href.toLowerCase().indexOf('/unified') > 0) {
//   $this.addClass('local-signup');

/*fix error box behavior*/
// $('.b2c-wrapper .error.itemLevel').each(function() {
//   $(this)
//     .next()
//     .next()
//     .after($(this));
// });
// $('input#email, input#newPassword').focus(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .addClass('hideError');
// });
// $('input#email, input#newPassword').blur(function() {
//   $(this)
//     .parent()
//     .find('.error')
//     .removeClass('hideError');
// });

// $('.local-signup .buttons').hide();
// $('.local-signup button#cancel').hide();

// $('.local-signup input')
//   .parent()
//   .hide();
// $('label[for="email"]').addClass('shrink');
// $('input#email')
//   .parent()
//   .show();
// $('input#email')
//   .parent()
//   .find('.buttons')
//   .show();
// $('input#email').focus();
// $('#email_ver_but_send').click(function() {
//   $('input#email')
//     .parent()
//     .find('.helpLink')
//     .hide();
//   $('label[for="email"], input#email').hide();
// });
// $('#email_ver_but_verify').click(function() {
//   $('.attrEntry.validate > div:first-child').css({
//     position: 'absolute',
//     top: '5rem'
//   });
//   $('#email_ver_input').after($('.attrEntry.validate > div:first-child'));
//   $('.working').show();
// setTimeout(function() {
//   $('.working').hide();
//   if (!$('.attrEntry .error').is(':visible')) {
//     $('input#email')
//       .parent()
//       .find('.helpLink')
//       .show();
//     $('label[for="email"], input#email').show();
//     $('input#newPassword')
//       .parent()
//       .show();
//     $('input#givenName')
//       .parent()
//       .show();
//     $('input#surname')
//       .parent()
//       .show();
//     $('input#newPassword').focus();
//     $('.local-signup .buttons').show();
//     $('button#continue').prop('disabled', false);
//     $('button#email_ver_but_edit').click(function() {
//       $('input#newPassword')
//         .parent()
//         .hide();
//       $('input#givenName')
//         .parent()
//         .hide();
//       $('input#surname')
//         .parent()
//         .hide();
//       $('.local-signup .buttons').hide();
//       $('.local-signup .buttons.verify').show();
//       $('input#email').prop('disabled', false);
//       $('input#email').focus();
//     });
//   }
// }, 2000);
//   });

//   $('input#newPassword').change(function() {
//     $('input#reenterPassword').val($('input#newPassword').val());
//   });
//   $('input#givenName, input#surname').change(function() {
//     $('input#displayName').val(
//       $('input#givenName').val() + ' ' + $('input#surname').val()
//     );
//   });
// } else if (location.href.toLowerCase().indexOf('/oauth2/authresp') > 0) {
// $this.addClass('social-signup');
// $('#attributeList ul').before($('.errorMessageSection'));
// $('#email_ver_but_default, #email_ver_but_send').click(function() {
//   $('.validate .error').removeClass('show');
//   $('label[for="email"]').addClass('shrink');
// });
// }

// $('input').focus(function() {
//   $('label[for="' + $(this).attr('id') + '"]').addClass('shrink');
// });
// $('input').blur(function() {
//   if ($(this).val() == '') {
//     $('label[for="' + $(this).attr('id') + '"]').removeClass('shrink');
//   }
// });
// $('input').on('change', function() {
//   if ($(this).val() == '') {
//     $('label[for="' + $(this).attr('id') + '"]').removeClass('shrink');
//   } else {
//     $('label[for="' + $(this).attr('id') + '"]').addClass('shrink');
//   }
// });

// /* apply field title style to autofill fields */
// $('input').each(function() {
//   if ($(this).val() != '') {
//     $('label[for="' + $(this).attr('id') + '"]').addClass('shrink');
//   }
// });

/* add show/hide password functionality */
// $('input[type="password"]').each(function() {
//   $(this).before('<div class="showHideButton"></div>');
// });
// $('.showHideButton').click(function() {
//   $this = $(this);
//   $target = $this.parent().find('input');
//   if ($target.attr('type') == 'password') {
//     $target.attr('type', 'text');
//     $this.addClass('hidePassword');
//   } else {
//     $target.attr('type', 'password');
//     $this.removeClass('hidePassword');
//   }
// });

/* add password strength meter */
// $('li.Password input[type="password"]').each(function() {
//   $(this).after(
//     '<div class="passwordStrength"><div class="strengthMeter"></div><div class="strengthScore"></div></div>'
//   );
// });
// $('li.Password input[type="password"]').keyup(function() {
//   updatePasswordScore(this, $(this).val());
// });

//   $('.working').hide();
//   $('.b2c-wrapper').show();

//   return this;
//   };
// })(jQuery);

// $(document).ready(function() {
//   console.log('js-active');
//   $('body').kcgov_b2c_override();
// });

console.log("B2C custom JS working...");
