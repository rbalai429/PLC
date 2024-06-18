[
    {
        "name": "Rich text editor",
        "alias": "rte",
        "view": "rte",
        "icon": "icon-article"
    },
    {
        "name": "Image",
        "nameTemplate": "{{ value && value.udi ? (value.udi | ncNodeName) : '' }}",
        "alias": "media",
        "view": "media",
        "icon": "icon-picture"
    },
    {
        "name": "Macro",
        "nameTemplate": "{{ value && value.macroAlias ? value.macroAlias : '' }}",
        "alias": "macro",
        "view": "macro",
        "icon": "icon-settings-alt"
    },
    {
        "name": "Embed",
        "alias": "embed",
        "view": "embed",
        "icon": "icon-movie-alt"
	},
	{
		"name": "Video",
		"alias": "video",
		"view": "video",
		"icon": "icon-movie-alt"
	},
    {
        "name": "Headline",
        "nameTemplate": "{{ value }}",
        "alias": "headline",
        "view": "textstring",
        "icon": "icon-coin",
        "config": {
            "style": "font-size: 36px; line-height: 45px; font-weight: bold",
            "markup": "<h1>#value#</h1>"
        }
    },
    {
        "name": "Quote",
        "nameTemplate": "{{ value ? value.substring(0,32) + (value.length > 32 ? '...' : '') : '' }}",
        "alias": "quote",
        "view": "textstring",
        "icon": "icon-quote",
        "config": {
            "style": "border-left: 3px solid #ccc; padding: 10px; color: #ccc; font-family: serif; font-style: italic; font-size: 18px",
            "markup": "<blockquote>#value#</blockquote>"
        }
	},
    {
        "name": "Header Section",
        "alias": "headerSection",
        "view": "/App_Plugins/DocTypeGridEditor/Views/doctypegrideditor.html",
        "render": "/App_Plugins/DocTypeGridEditor/Render/DocTypeGridEditor.cshtml",
        "icon": "icon-picture",
        "config": {
            "allowedDocTypes": ["^headerSection$"],
            "nameTemplate": "headerSection",
            "enablePreview": true,
            "viewPath": "/Views/Partials/Grid/Editors/",
            "previewViewPath": "/Views/Partials/Grid/Editors/",
            "previewCssFilePath": "",
            "previewJsFilePath": ""
        }
    },
    {
        "name": "Our Education Section",
        "alias": "educationPanel",
        "view": "/App_Plugins/DocTypeGridEditor/Views/doctypegrideditor.html",
        "render": "/App_Plugins/DocTypeGridEditor/Render/DocTypeGridEditor.cshtml",
        "icon": "icon-picture",
        "config": {
            "allowedDocTypes": ["^educationPanel$"],
            "nameTemplate": "educationPanel",
            "enablePreview": true,
            "viewPath": "/Views/Partials/Grid/Editors/",
            "previewViewPath": "/Views/Partials/Grid/Editors/",
            "previewCssFilePath": "",
            "previewJsFilePath": ""
        }
    },
    {
        "name": "Core Philosophy",
        "alias": "corePhilosophy",
        "view": "/App_Plugins/DocTypeGridEditor/Views/doctypegrideditor.html",
        "render": "/App_Plugins/DocTypeGridEditor/Render/DocTypeGridEditor.cshtml",
        "icon": "icon-picture",
        "config": {
            "allowedDocTypes": ["^corePhilosophy$"],
            "nameTemplate": "corePhilosophy",
            "enablePreview": true,
            "viewPath": "/Views/Partials/Grid/Editors/",
            "previewViewPath": "/Views/Partials/Grid/Editors/",
            "previewCssFilePath": "",
            "previewJsFilePath": ""
        }
    }
    ,
    {
        "name": "Print Learn Center",
        "alias": "printLearnCenter",
        "view": "/App_Plugins/DocTypeGridEditor/Views/doctypegrideditor.html",
        "render": "/App_Plugins/DocTypeGridEditor/Render/DocTypeGridEditor.cshtml",
        "icon": "icon-picture",
        "config": {
            "allowedDocTypes": ["^printLearnCenter$"],
            "nameTemplate": "printLearnCenter",
            "enablePreview": true,
            "viewPath": "/Views/Partials/Grid/Editors/",
            "previewViewPath": "/Views/Partials/Grid/Editors/",
            "previewCssFilePath": "",
            "previewJsFilePath": ""
        }
    },
    {
        "name": "Members Speak",
        "alias": "membersSpeak",
        "view": "/App_Plugins/DocTypeGridEditor/Views/doctypegrideditor.html",
        "render": "/App_Plugins/DocTypeGridEditor/Render/DocTypeGridEditor.cshtml",
        "icon": "icon-picture",
        "config": {
            "allowedDocTypes": ["^membersSpeak$"],
            "nameTemplate": "membersSpeak",
            "enablePreview": true,
            "viewPath": "/Views/Partials/Grid/Editors/",
            "previewViewPath": "/Views/Partials/Grid/Editors/",
            "previewCssFilePath": "",
            "previewJsFilePath": ""
        }
    }
    
]
