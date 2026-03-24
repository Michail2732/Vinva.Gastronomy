<template>
    <div class="auth-layout">
        <!-- Фоновое изображение с градиентом -->
        <div class="auth-background"></div>

        <!-- Контент -->
        <div class="auth-container">
            <!-- Логотип -->
            <div class="auth-logo">
                <Image src="/logo.svg" alt="logo" class="logo-img" />
            </div>

            <!-- Карточка с формой -->
            <div class="auth-card">
                <router-view class="card-content" />
            </div>

            <!-- Ссылка на главную -->
            <div class="auth-footer">
                <router-link to="/" class="back-link">
                    <i class="pi pi-home"></i>
                    Вернуться на главную
                </router-link>
            </div>
        </div>
    </div>
</template>
<script lang="ts">

export default {

}
</script>
<style scoped lang="scss">
.auth-layout {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background: linear-gradient(135deg, #f97316 0%, #fbbf24 100%);
}

.auth-background {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;    
    background-repeat: repeat;
    opacity: 0.3;
}

.auth-container {
    width: 100%;
    max-width: 450px;
    padding: 1.5rem;
    position: relative;
    z-index: 1;

    .auth-logo {        
        margin-bottom: 2rem;
        .logo-img :deep(img)
        {
            max-width: 400px;
            margin: 0 auto;
            color: white;
        }
    }

    .auth-card {
        background: white;
        border-radius: 1rem;
        padding: 2rem;
        box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);

        .card-content {
            margin: 0 auto;
        }
    }

    .auth-footer {
        text-align: center;
        margin-top: 1.5rem;

        .back-link {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            color: white;
            text-decoration: none;
            font-size: 0.875rem;
            transition: opacity 0.2s;

            &:hover {
                opacity: 0.8;
            }

            i {
                font-size: 0.875rem;
            }
        }
    }
}


@media (max-width: 640px) {
    .auth-container {
        padding: 1rem;
    }

    .auth-card {
        padding: 1.5rem;
    }

    .auth-logo .logo-link {
        font-size: 1.25rem;

        .logo-icon {
            font-size: 1.5rem;
        }
    }
}
</style>