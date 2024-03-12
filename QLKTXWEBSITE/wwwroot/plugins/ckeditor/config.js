/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	config.syntaxhighlight_lang = 'csharp';
	config.syntaxhighlight_hideControls = true;
	config.language = 'vi';
	config.filebrowserBrowseUrl = '/wwwroot/plugins/ckfinder/ckfinder.html';
	config.filebrowserImageBrowseUrl = '/wwwroot/plugins/ckfinder/ckfinder.html?Type=Images';
	config.filebrowserFlashBrowseUrl = 'wwwroot/plugins/ckfinder/ckfinder.html?Type=Flash';
	config.filebrowserUploadUrl = '/wwwroot/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
	config.filebrowserImageUploadUrl = '/Data ';
	config.filebrowserflashUploadUrl = '/wwwroot/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

	CKFinder.setupCKEditor(null,'/wwwroot/plugins/ckfinder/');
};
