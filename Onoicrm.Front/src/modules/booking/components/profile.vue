<template>
  <div :class="className" class="user-profile">
    <div class="user-profile__box">
      <h1 class="user-profile__initials">{{ getInitials(userStore.profile?.fullName) }}</h1>
    </div>
    <div class="ml-2">
      <h2 class="user-profile__name w-8rem">{{userStore.profile?.fullName}}</h2>
      <p class="user-profile__profession">{{ userStore.roleCaption }}</p>
    </div>
  </div>
</template>
<script lang="ts" setup>
import {useUserStore} from "~/modules/user/stores/useUserStore";
import {getInitials} from "~/modules/user-profile/services/getInitials";
withDefaults(defineProps<{className?:string}>(),{
  className:""
})
const userStore = useUserStore()
</script>

