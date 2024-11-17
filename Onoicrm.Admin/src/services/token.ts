import jwtDecode from 'jwt-decode';

export const tokenName = 'denttish-jwt-token';

export const isJwtValid = (jwt: string | undefined | null): boolean => {
    if (!jwt || jwt === 'undefined') return false;
    const jwtData: any = jwtDecode(jwt) || {};
    const expires = jwtData.exp || 0;
    return new Date().getTime() / 1000 < expires;
};
