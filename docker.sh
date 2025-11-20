#!/bin/bash

# HP Clima API - Docker Helper Script
# Este script facilita o gerenciamento da aplicação via Docker

set -e

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Função para exibir menu
show_menu() {
    echo -e "${BLUE}================================================${NC}"
    echo -e "${BLUE}  HP Clima API - Docker Management${NC}"
    echo -e "${BLUE}================================================${NC}"
    echo ""
    echo "1) Build - Criar imagem Docker"
    echo "2) Up - Iniciar todos os serviços"
    echo "3) Down - Parar todos os serviços"
    echo "4) Logs - Ver logs da aplicação"
    echo "5) Restart - Reiniciar serviços"
    echo "6) Clean - Limpar containers e volumes"
    echo "7) Status - Ver status dos containers"
    echo "8) Shell - Acessar shell do container da API"
    echo "9) Test - Testar endpoints da API"
    echo "0) Sair" 
    echo ""
}

# Build da imagem
build() {
    echo -e "${YELLOW}🔨 Building Docker image...${NC}"
    docker-compose build --no-cache
    echo -e "${GREEN}✅ Build completed!${NC}"
}

# Iniciar serviços
up() {
    echo -e "${YELLOW}🚀 Starting services...${NC}"
    docker-compose up -d
    echo -e "${GREEN}✅ Services started!${NC}"
    echo -e "${BLUE}API: http://localhost:5109${NC}"
    echo -e "${BLUE}MongoDB: mongodb://admin:admin123@localhost:27017${NC}"
}

# Parar serviços
down() {
    echo -e "${YELLOW}🛑 Stopping services...${NC}"
    docker-compose down
    echo -e "${GREEN}✅ Services stopped!${NC}"
}

# Ver logs
logs() {
    echo -e "${YELLOW}📋 Showing logs...${NC}"
    docker-compose logs -f --tail=100
}

# Reiniciar
restart() {
    echo -e "${YELLOW}🔄 Restarting services...${NC}"
    docker-compose restart
    echo -e "${GREEN}✅ Services restarted!${NC}"
}

# Limpar tudo
clean() {
    echo -e "${RED}⚠️  This will remove all containers, volumes and images!${NC}"
    read -p "Are you sure? (y/N): " confirm
    if [[ $confirm == [yY] ]]; then
        echo -e "${YELLOW}🧹 Cleaning up...${NC}"
        docker-compose down -v
        docker system prune -f
        echo -e "${GREEN}✅ Cleanup completed!${NC}"
    else
        echo -e "${BLUE}Cancelled.${NC}"
    fi
}

# Status
status() {
    echo -e "${YELLOW}📊 Container status:${NC}"
    docker-compose ps
    echo ""
    echo -e "${YELLOW}📊 Resource usage:${NC}"
    docker stats --no-stream hp-clima-api hp-clima-mongodb
}

# Acessar shell
shell() {
    echo -e "${YELLOW}🐚 Accessing container shell...${NC}"
    docker exec -it hp-clima-api /bin/sh
}

# Testar API
test_api() {
    echo -e "${YELLOW}🧪 Testing API endpoints...${NC}"
    echo ""
    
    echo -e "${BLUE}1. Health Check:${NC}"
    curl -s http://localhost:5109/health | jq '.' || echo "OK"
    echo ""
    
    echo -e "${BLUE}2. Registering CEP (01311000):${NC}"
    curl -s -X POST http://localhost:5109/api/cep \
        -H "Content-Type: application/json" \
        -d '{"zipCode": "01311000"}' | jq '.'
    echo ""
    
    echo -e "${BLUE}3. Getting Weather (3 days):${NC}"
    curl -s http://localhost:5109/api/weather?days=3 | jq '.'
    echo ""
    
    echo -e "${GREEN}✅ Tests completed!${NC}"
}

# Main
main() {
    while true; do
        show_menu
        read -p "Escolha uma opção: " choice
        echo ""
        
        case $choice in
            1) build ;;
            2) up ;;
            3) down ;;
            4) logs ;;
            5) restart ;;
            6) clean ;;
            7) status ;;
            8) shell ;;
            9) test_api ;;
            0) 
                echo -e "${GREEN}Goodbye!${NC}"
                exit 0
                ;;
            *)
                echo -e "${RED}Invalid option!${NC}"
                ;;
        esac
        
        echo ""
        read -p "Press Enter to continue..."
    done
}

# Executar
main
