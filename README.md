# ToyProject_team8

쿠키런 스타일의 2D 횡스크롤 러닝 게임 (Unity 토이 프로젝트)

## 프로젝트 소개

플레이어가 쿠키 캐릭터를 조작하여 장애물을 피하고 아이템을 획득하며 최대한 멀리 달리는 횡스크롤 러닝 게임입니다. 가챠 시스템을 통해 새로운 쿠키와 유물을 획득할 수 있습니다.

- **엔진**: Unity 6000.3.11f1
- **플랫폼**: Mobile (Android)
- **장르**: 2D 횡스크롤 러닝 / 캐주얼

## 주요 기능

### 게임플레이
- **점프 & 슬라이드**: 두 가지 액션으로 장애물 회피
- **다중 스테이지**: 젤리벌레 숲, 머나먼 바다 등 자동 전환 스테이지
- **점수 & 코인 시스템**: 달린 거리 점수와 코인 획득

### 캐릭터 시스템
- **3종 쿠키**: 용감한 쿠키(Hero), 체리맛 쿠키(Cherry), 해적 쿠키(Pirate)
- 각 쿠키별 고유 능력 및 동작 (CookieBehaviorFactory 기반)

### 아이템 & 유물
- **소비 아이템**: 자석, 대시, 거대화, 골드젤리, 코인 변환, 체력 포션
- **장착 유물(Gear)**: 자석 유물, 드링크 유물, 코인 플라워 유물
- **3개 슬롯**: 최대 3개의 유물을 동시 장착

### 가챠 시스템
- 쿠키 및 유물 뽑기
- 박스 연출, 회전 이펙트, 보상 연출

### 데이터 저장
- 쿠키/유물 보유 정보 저장 및 로드
- 데이터 테이블 기반 캐릭터/아이템 관리

## 프로젝트 구조

```
toy-project-0-team-8/
├── Assets/
│   ├── Scenes/                  # 씬 파일
│   │   ├── Lobby.unity              # 로비 씬
│   │   ├── GachaScene.unity         # 가챠 씬
│   │   ├── ReadyScene.unity         # 게임 준비 씬
│   │   ├── PlayScene.unity          # 인게임 씬
│   │   ├── GameResultScene.unity    # 결과 씬
│   │   ├── Stage2JellyBugForest.unity
│   │   └── Stage3FarawayOceans.unity
│   │
│   ├── Scripts/
│   │   ├── Character/           # 쿠키 캐릭터 (Hero, Cherry, Pirate)
│   │   ├── Constants/           # 태그, Enum 등 상수
│   │   ├── Data/                # 데이터 테이블, 세이브/로드
│   │   ├── GamePlay/            # 인게임 매니저, 버튼, 자석영역
│   │   ├── GameReady/           # 게임 준비 매니저
│   │   ├── GameResult/          # 결과 매니저
│   │   ├── Item/
│   │   │   ├── Object/              # 소비 아이템 (Coin, Jelly, Potion 등)
│   │   │   └── Gear/                # 장착 유물 (Magnet, Drink, CoinFlower)
│   │   ├── Map/                 # 스테이지, 맵 프리팹
│   │   └── Ui/
│   │       ├── Lobby/               # 로비 UI
│   │       ├── Play/                # 인게임 UI
│   │       └── Gacha/               # 가챠 UI
│   │
│   ├── Imported/                # 외부 GUI 에셋 (Mobile Hyper-Casual)
│   ├── Prefabs/                 # 프리팹
│   └── Resources/               # 런타임 리소스
│
├── ProjectSettings/             # Unity 프로젝트 설정
└── Packages/                    # 패키지 의존성
```

## 핵심 스크립트

- `GameManager.cs` — 인게임 전체 흐름 제어
- `CookieController.cs` — 쿠키 캐릭터 입력/상태 관리
- `CookieBehaviorFactory.cs` — 쿠키별 행동 생성
- `SaveLoadManager.cs` — 저장 데이터 관리
- `DataTableManager.cs` — 쿠키/유물 데이터 테이블 관리
- `GachaManager.cs` — 가챠 시스템 관리
- `StageChanger.cs` — 스테이지 전환

## 팀

Unity Class — Team 8
