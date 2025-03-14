const fs = require('fs');
const path = require('path');
const glob = require('glob');

// PrimeNG component mapping: Supports tag, class, and attribute usage
const primeNgStandaloneMap = {
  // Form Components
  'primeng/autocomplete': { tag: 'p-autoComplete', import: 'AutoComplete' },
  'primeng/datepicker': { tag: 'p-datePicker', import: 'DatePicker' }, // Formerly 'Calendar'
  'primeng/cascadeselect': { tag: 'p-cascadeSelect', import: 'CascadeSelect' },
  'primeng/checkbox': { tag: 'p-checkbox', import: 'Checkbox' },
  'primeng/chips': { tag: 'p-chips', import: 'Chips' },
  'primeng/colorpicker': { tag: 'p-colorPicker', import: 'ColorPicker' },
  'primeng/select': { tag: 'p-select', import: 'Select' }, // formerly 'Dropdown'
  'primeng/editor': { tag: 'p-editor', import: 'Editor' },
  'primeng/inputmask': { tag: 'p-inputMask', import: 'InputMask' },
  'primeng/toggleswitch': { tag: 'p-toggleSwitch', import: 'ToggleSwitch' }, // formerly 'InputSwitch'
  'primeng/inputtext': {
    tag: 'input',
    class: 'p-inputtext',
    attr: 'pInputText',
    import: 'InputText',
  },
  'primeng/inputtextarea': {
    tag: 'textarea',
    class: 'p-inputtextarea',
    attr: 'pInputTextarea',
    import: 'InputTextarea',
  },
  'primeng/inputnumber': { tag: 'p-inputNumber', import: 'InputNumber' },
  'primeng/knob': { tag: 'p-knob', import: 'Knob' },
  'primeng/keyfilter': { attr: 'pKeyFilter', import: 'KeyFilter' },
  'primeng/listbox': { tag: 'p-listbox', import: 'Listbox' },
  'primeng/multiselect': { tag: 'p-multiSelect', import: 'MultiSelect' },
  'primeng/password': { tag: 'p-password', import: 'Password' },
  'primeng/radiobutton': { tag: 'p-radioButton', import: 'RadioButton' },
  'primeng/rating': { tag: 'p-rating', import: 'Rating' },
  'primeng/slider': { tag: 'p-slider', import: 'Slider' },
  'primeng/selectbutton': { tag: 'p-selectButton', import: 'SelectButton' },
  'primeng/togglebutton': { tag: 'p-toggleButton', import: 'ToggleButton' },
  'primeng/treeselect': { tag: 'p-treeSelect', import: 'TreeSelect' },

  // Button Components
  'primeng/button': {
    tag: 'p-button',
    class: 'p-button',
    attr: 'pButton',
    import: 'Button',
  },
  'primeng/splitbutton': { tag: 'p-splitButton', import: 'SplitButton' },
  'primeng/speeddial': { tag: 'p-speedDial', import: 'SpeedDial' },

  // Data Components
  'primeng/dataview': { tag: 'p-dataView', import: 'DataView' },
  'primeng/gmap': { tag: 'p-gMap', import: 'GMap' },
  'primeng/orderlist': { tag: 'p-orderList', import: 'OrderList' },
  'primeng/organizationchart': {
    tag: 'p-organizationChart',
    import: 'OrganizationChart',
  },
  'primeng/paginator': { tag: 'p-paginator', import: 'Paginator' },
  'primeng/picklist': { tag: 'p-pickList', import: 'PickList' },
  'primeng/table': { tag: 'p-table', import: 'Table' },
  'primeng/timeline': { tag: 'p-timeline', import: 'Timeline' },
  'primeng/tree': { tag: 'p-tree', import: 'Tree' },
  'primeng/treetable': { tag: 'p-treeTable', import: 'TreeTable' },
  'primeng/virtualscroller': {
    tag: 'p-virtualScroller',
    import: 'VirtualScroller',
  },
  'primeng/scroller': { tag: 'p-scroller', import: 'Scroller' },

  // Panel Components
  'primeng/accordion': { tag: 'p-accordion', import: 'Accordion' },
  'primeng/card': { tag: 'p-card', import: 'Card' },
  'primeng/divider': { tag: 'p-divider', import: 'Divider' },
  'primeng/fieldset': { tag: 'p-fieldset', import: 'Fieldset' },
  'primeng/panel': { tag: 'p-panel', import: 'Panel' },
  'primeng/splitter': { tag: 'p-splitter', import: 'Splitter' },
  'primeng/scrollpanel': { tag: 'p-scrollPanel', import: 'ScrollPanel' },
  'primeng/tabs': { tag: 'p-tabs', import: 'Tabs' }, // formerly 'TabView'
  'primeng/toolbar': { tag: 'p-toolbar', import: 'Toolbar' },

  // Overlay Components
  'primeng/confirmdialog': { tag: 'p-confirmDialog', import: 'ConfirmDialog' },
  'primeng/confirmpopup': { tag: 'p-confirmPopup', import: 'ConfirmPopup' },
  'primeng/dialog': { tag: 'p-dialog', import: 'Dialog' },
  'primeng/dynamicdialog': { tag: 'p-dynamicDialog', import: 'DynamicDialog' },
  'primeng/popover': { tag: 'p-popover', import: 'Popover' }, // formerly 'OverlayPanel'
  'primeng/drawer': { tag: 'p-drawer', import: 'Drawer' }, // formerly 'Sidebar'
  'primeng/tooltip': {
    class: 'p-tooltip',
    attr: 'pTooltip',
    import: 'Tooltip',
  },

  // File Components
  'primeng/fileupload': { tag: 'p-fileUpload', import: 'FileUpload' },

  // Menu Components
  'primeng/menu': { tag: 'p-menu', import: 'Menu' },
  'primeng/breadcrumb': { tag: 'p-breadcrumb', import: 'Breadcrumb' },
  'primeng/contextmenu': { tag: 'p-contextMenu', import: 'ContextMenu' },
  'primeng/dock': { tag: 'p-dock', import: 'Dock' },
  'primeng/megamenu': { tag: 'p-megaMenu', import: 'MegaMenu' },
  'primeng/menubar': { tag: 'p-menubar', import: 'Menubar' },
  'primeng/panelmenu': { tag: 'p-panelMenu', import: 'PanelMenu' },
  'primeng/slidemenu': { tag: 'p-slideMenu', import: 'SlideMenu' },
  'primeng/steps': { tag: 'p-steps', import: 'Steps' },
  'primeng/tabmenu': { tag: 'p-tabMenu', import: 'TabMenu' },
  'primeng/tieredmenu': { tag: 'p-tieredMenu', import: 'TieredMenu' },

  // Messages & Notifications
  'primeng/messagelist': { tag: 'p-message', import: 'Message' },
  'primeng/messages': { tag: 'p-messages', import: 'Messages' },
  'primeng/toast': { tag: 'p-toast', import: 'Toast' },

  // Miscellaneous Components
  'primeng/dragdrop': { attr: 'pDraggable', import: 'DragDrop' },
  'primeng/focustrap': { attr: 'pFocusTrap', import: 'FocusTrap' },
  'primeng/inplace': { tag: 'p-inplace', import: 'Inplace' },
  'primeng/progressbar': { tag: 'p-progressBar', import: 'ProgressBar' },
  'primeng/progressspinner': {
    tag: 'p-progressSpinner',
    import: 'ProgressSpinner',
  },
  'primeng/skeleton': { tag: 'p-skeleton', import: 'Skeleton' },
  'primeng/tag': { tag: 'p-tag', import: 'Tag' },
  'primeng/terminal': { tag: 'p-terminal', import: 'Terminal' },
};

class ComponentData {
  constructor(componentPath) {
    this.path = componentPath;
    this.fileName = path.basename(componentPath);
  }
}

// Find all component .ts files
const componentsData = glob
  .sync('src/app/**/*.component.ts')
  .map(componentPath => new ComponentData(componentPath));

// Map component files to their corresponding HTML templates
const componentTemplateMap = new Map();

// Scan .ts files to find external templates (`templateUrl`)
componentsData.forEach(componentData => {
  const content = fs.readFileSync(componentData.path, 'utf8');
  const match = content.match(/templateUrl:\s*['"]([^'"]+)['"]/);

  if (match) {
    const templatePath = path.resolve(
      path.dirname(componentData.path),
      match[1]
    );
    if (fs.existsSync(templatePath)) {
      componentTemplateMap.set(componentData.path, templatePath);
    }
  }
});

// Function to extract PrimeNG components from an HTML template
function extractPrimeNGComponents(templateContent) {
  const usedComponents = new Set();

  // Regex patterns to catch all PrimeNG usages
  const tagRegex = /<(p-[a-zA-Z0-9-]+)/g;
  const classRegex = /class=["'][^"']*\b(p-[a-zA-Z0-9-]+)\b[^"']*["']/g;
  const attrRegex = /\b(p[A-Z][a-zA-Z0-9]*)\b/g;

  let match;

  // Tag-based components (e.g., `<p-select>`)
  while ((match = tagRegex.exec(templateContent)) !== null) {
    const component = match[1];
    Object.values(primeNgStandaloneMap).forEach(
      ({ tag, import: componentImport }) => {
        if (component === tag) {
          usedComponents.add(componentImport);
        }
      }
    );
  }

  // Class-based components (e.g., `class="p-float-label"`)
  while ((match = classRegex.exec(templateContent)) !== null) {
    const component = match[1];
    Object.values(primeNgStandaloneMap).forEach(
      ({ class: className, import: componentImport }) => {
        if (component === className) {
          usedComponents.add(componentImport);
        }
      }
    );
  }

  // Attribute-based components (e.g., `pButton`, `pTemplate`)
  while ((match = attrRegex.exec(templateContent)) !== null) {
    const component = match[1];
    Object.values(primeNgStandaloneMap).forEach(
      ({ attr, import: componentImport }) => {
        if (component === attr) {
          usedComponents.add(componentImport);
        }
      }
    );
  }

  return usedComponents;
}

//const debugComponentsData = componentsData.splice(0, 3);

// Process each component
componentsData.forEach(componentData => {
  let componentContent = fs.readFileSync(componentData.path, 'utf8');
  let templateContent = '';

  // Check if component uses an external template
  if (componentTemplateMap.has(componentData.path)) {
    templateContent = fs.readFileSync(
      componentTemplateMap.get(componentData.path),
      'utf8'
    );
  } else {
    // Detect inline template: `template: '<button pButton>'` or `template: "..."`
    const inlineTemplateMatch = componentContent.match(
      /template:\s*`([\s\S]*?)`|template:\s*"([\s\S]*?)"/
    );
    if (inlineTemplateMatch) {
      templateContent = inlineTemplateMatch[1] || inlineTemplateMatch[2] || '';
    }
  }

  if (!templateContent) {
    console.log(`⚠️  ${componentData.fileName} : no template content`);
    return; // Skip if no template found
  }

  const usedPrimeNGComponents = extractPrimeNGComponents(templateContent);
  if (usedPrimeNGComponents.size === 0) {
    console.log(`⛔ ${componentData.fileName} : no primeng components used`);
    return; // Skip if no PrimeNG components found
  }

  // Check if the component is already standalone
  const isStandalone = !componentContent.includes('standalone: false');
  const existingImportsMatch = componentContent.match(
    /imports:\s*\[([\s\S]*?)\]/
  );
  let existingImports = existingImportsMatch
    ? existingImportsMatch[1].split(', ')
    : [];

  const newImports = Array.from(usedPrimeNGComponents).filter(
    c => !existingImports.includes(c)
  );

  if (newImports.length === 0) {
    console.log(`✅ ${componentData.fileName} : already up to date`);
    return; // No new imports needed
  }

  // Generate import statements
  const newImportStatements = newImports
    .map(c => `import { ${c} } from 'primeng/${c.toLowerCase()}';`)
    .join('\n');

  // Inject standalone flag and imports
  if (isStandalone) {
    // Add missing imports inside the `imports: []` array
    componentContent = componentContent.replace(
      /imports:\s*\[([\s\S]*?)\]/,
      (match, imports) =>
        `imports: [${imports.trim()}${imports.trim() ? ', ' : ''}${newImports.join(', ')}]`
    );
  } else {
    // If not standalone, add `standalone: true` and the imports
    componentContent = componentContent.replace(
      /@Component\({/,
      `@Component({\n  imports: [${newImports.join(', ')}],`
    );
  }

  // Ensure new imports are not duplicated
  if (!componentContent.includes(newImportStatements)) {
    componentContent = newImportStatements + '\n' + componentContent;
  }

  // Update the component file with new imports
  //fs.writeFileSync(componentData.path, componentContent);
  console.log(`🔄 ${componentData.fileName} : updated`);
});

console.log('🚀 Migration complete!');
