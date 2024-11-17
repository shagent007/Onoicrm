import { Vue, Component, Prop } from "vue-property-decorator";
import docx from "./docx.vue";
import file from "./file.vue";
import pdf from "./pdf.vue";
import pptx from "./pptx.vue";
import txt from "./txt.vue";
import zip from "./zip.vue";
import xlsx from "./xlsx.vue";

@Component({
  components: { docx, file, pdf, pptx, txt, zip, xlsx },
})
export default class FilePreviews extends Vue {}
