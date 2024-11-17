<template>
  <nav class="login">
    <div  class="login__box">
      <div class="text-center flex align-items-center">
        <router-link to="/">
          <svg
              xmlns="http://www.w3.org/2000/svg"
              width="41"
              height="44"
              viewBox="0 0 41 44"
              fill="none"
          >
            <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M6.47368 0.0357056C2.89837 0.0357056 0 2.93407 0 6.50938V37.4906C0 41.0659 2.89836 43.9643 6.47368 43.9643H19.0357C31.1663 43.9643 41 34.1305 41 22C41 9.86945 31.1663 0.0357056 19.0357 0.0357056H6.47368ZM9.78102 23.2836C9.31382 23.0738 8.78571 23.4156 8.78571 23.9277V26.5361C8.78571 26.8308 8.95868 27.0981 9.22753 27.2188C15.4663 30.0199 22.6051 30.0199 28.8439 27.2188C29.1127 27.0981 29.2857 26.8308 29.2857 26.5361V23.9277C29.2857 23.4156 28.7576 23.0738 28.2904 23.2836C22.4037 25.9266 15.6678 25.9266 9.78102 23.2836Z"
                fill="#1479FF"
            />
          </svg>
        </router-link>
        <p class="login__subtitle">onoicrm</p>
      </div>
      <div class="buttons">
        <Button class="btn-login btn-login--gray" label="Помощь" @click="visible = false" text />
        <Button
            class="btn-login"
            label="Регистрация"
        />
      </div>
    </div>
    <div class="login__body" >
      <slot></slot>
    </div>
  </nav>
</template>
