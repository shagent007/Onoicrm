<template>
  <table class="base-table">
    <thead>
      <tr>
        <th v-for="column in columns" :key="column.id + 'th'">
          {{column.caption}}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in items" :key="item[config.keyName]">
        <td v-for="column in columns" :data-label="column.caption" :key="column.id + 'th'">
          <slot :name="column.name" :item="item">{{getValue(item, column.name)}}</slot>
        </td>
      </tr>
    </tbody>
  </table>



</template>
<script lang="ts" setup>
import {BaseTableColumn} from "../models/base-table/BaseTableColumn";
import {BaseTableConfig} from "../models/base-table/BaseTableConfig";
import {getValue} from "../lib/helpers";
interface PropTypes{
  items:any[],
  columns:BaseTableColumn[],
  config?:BaseTableConfig
}

const {} = withDefaults(defineProps<PropTypes>(),{
  config:()=> new BaseTableConfig()
});
</script>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "base-table",
  inheritAttrs: false,
  customOptions: {},
});
</script>
