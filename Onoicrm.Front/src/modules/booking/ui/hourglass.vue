<script setup lang="ts">
import moment from "moment";

const {arg} = defineProps<{arg:any}>();
const isCompact = ({event}:any) => {
  const start = moment(event.start);
  const end = moment(event.end);
  const diff = end.diff(start, "minutes")
  return diff <= 30;
}
</script>

<template>

</template>

<style scoped lang="scss">

</style>
